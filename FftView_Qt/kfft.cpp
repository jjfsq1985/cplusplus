#include "math.h"  
#include "kfft.h"


FourierTransform::FourierTransform()
    :m_nWindow(1)
{
    m_pwin = nullptr;
    m_pwt = nullptr;
}

FourierTransform::~FourierTransform()
{
    ReleaseFft();
}

//nCount == 2^k ,要求整数幂，kfft会修改原始时域输入数据
//pReal实部，pImage虚部，nCount数据量 k蝶形运算等级
//fr、fi是FFT的实部和虚部
//il==1则pReal,pImage为频域输出值(模和幅角)
//Inverse=0正变换 FFT，Inverse=1逆变换 IFFT
void FourierTransform::kfft(float *pReal, float *pImage, int nCount, float *fr, float *fi, int Inverse, int il)
{
    int k = NumberOfBits(nCount);
    if (k == -1)
        return;
    int it, m, is, i, j, nv, ll;
    float p, q, s;
    float vr, vi, poddr, poddi;
    //数据按k蝶形排序
    for (it = 0; it <= nCount - 1; it++)
    {
        m = it;
        is = 0;
        for (i = 0; i <= k - 1; i++)
        {
            j = m / 2; 
            is = 2 * is + (m - 2 * j); 
            m = j;
        }
        fr[it] = pReal[is];
        fi[it] = pImage[is];
    }
    pReal[0] = 1.0f;
    pImage[0] = 0.0f;
    p = 2.0f * M_PI / (1.0f*nCount);
    pReal[1] = cos(p);
    pImage[1] = -sin(p);
    if (Inverse != 0)
    {
        pImage[1] = -pImage[1];//逆变换
    }
    //权值运算
    for (i = 2; i <= nCount - 1; i++)
    {
        p = pReal[i - 1] * pReal[1]; 
        q = pImage[i - 1] * pImage[1];
        s = (pReal[i - 1] + pImage[i - 1])*(pReal[1] + pImage[1]);
        pReal[i] = p - q; 
        pImage[i] = s - p - q;
    }
    for (it = 0; it <= nCount - 2; it = it + 2)
    {
        vr = fr[it]; 
        vi = fi[it];
        fr[it] = vr + fr[it + 1];
        fi[it] = vi + fi[it + 1];
        fr[it + 1] = vr - fr[it + 1]; 
        fi[it + 1] = vi - fi[it + 1];
    }
    m = nCount / 2; 
    nv = 2;
    for (ll = k - 2; ll >= 0; ll--)
    {
        m = m / 2; 
        nv = 2 * nv;
        for (it = 0; it <= (m - 1)*nv; it = it + nv)
        {
            for (j = 0; j <= (nv / 2) - 1; j++)
            {
                p = pReal[m*j] * fr[it + j + nv / 2];
                q = pImage[m*j] * fi[it + j + nv / 2];
                s = pReal[m*j] + pImage[m*j];
                s = s*(fr[it + j + nv / 2] + fi[it + j + nv / 2]);
                poddr = p - q; 
                poddi = s - p - q;
                fr[it + j + nv / 2] = fr[it + j] - poddr;
                fi[it + j + nv / 2] = fi[it + j] - poddi;
                fr[it + j] = fr[it + j] + poddr;
                fi[it + j] = fi[it + j] + poddi;
            }
        }
    }
    if (Inverse != 0)
    {
        //逆变换要乘以系数1/nCount
        for (i = 0; i <= nCount - 1; i++)
        {
            fr[i] = fr[i] / (1.0f*nCount);
            fi[i] = fi[i] / (1.0f*nCount);
        }
    }
    if (il != 0)
    {
        for (i = 0; i <= nCount - 1; i++)
        {
           //模和幅角（角度制）
            pReal[i] = sqrt(fr[i] * fr[i] + fi[i] * fi[i]);
            if (fabs(fr[i]) < 0.000001*fabs(fi[i]))
            {
                if ((fi[i] * fr[i]) > 0)
                {
                    pImage[i] = 90.0;
                }
                else
                {
                    pImage[i] = -90.0;
                }
            }
            else
            {
                pImage[i] = atan(fi[i] / fr[i])*180.0f / M_PI;
            }
        }
    }
}

int FourierTransform::NumberOfBits(int powerofTwo)
{
    for (int i = 0; i <= 20; i++)
    {
       if ( (powerofTwo & (1 << i)) != 0)
            return i;
    }
    return -1;
}

//码位倒序转换
unsigned FourierTransform::BitReverisee(unsigned int x, int log2n)
{
    unsigned int n = 0;
    for (int i = 0; i < log2n; i++)
    {
        n <<= 1;
        n |= (x & 1);
        x >>= 1;
    }
    return n;
}

bool FourierTransform::InitFft(int nCount)
{
    //不是整数幂，或数据量超过2^20
    if (FourierTransform::NumberOfBits(nCount) == -1)
        return false;
    m_nCount = nCount;
    int i = 0;
    m_pwin = new float[m_nCount];
    m_pwt = new complex_f[m_nCount];
    for (i = 0; i < m_nCount; i++)
    {
        m_pwin[i] = float(0.5 - 0.5*cos(2*M_PI *i/(m_nCount-1))); //汉宁窗
    }

    for (i = 0; i < m_nCount; i++)
    {
        float angle = -i*M_PI * 2 / m_nCount;
        m_pwt[i] = complex_f(cos(angle), sin(angle));
    }
    return true;
}

void FourierTransform::ReleaseFft()
{
    if (m_pwin != nullptr)
        delete[] m_pwin;
    m_pwin = nullptr;
    if (m_pwt != nullptr)
        delete[] m_pwt;
    m_pwt = nullptr;
}

bool FourierTransform::FFT(complex_f *data, int nCount)
{
    if (!InitFft(nCount))
        return false;
    int i, j, k, butterfly, p;
    int power = FourierTransform::NumberOfBits(nCount);
    for (k = 0; k < power; k++)
    {
        int powerk = 1 << k;
        for (j = 0; j < powerk; j++)
        {
            butterfly = 1 << (power - k);
            p = j*butterfly;
            int s = p + butterfly / 2;
            for (i = 0; i < butterfly / 2; i++)
            {
                complex_f t = data[i + p] + data[i + s];
                data[i + s] = (data[i + p] - data[i + s]) * m_pwt[i*powerk];
                data[i + p] = t;
            }
        }
    }
    for (k = 0; k < nCount; k++)
    {
        int r = BitReverisee(k, power);
        if (r > k)
        {
            complex_f t = data[k];
            data[k] = data[r];
            data[r] = t;
        }
    }
    ReleaseFft();
    return true;
}
