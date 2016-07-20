#pragma once

const double M_PI = 3.141592653589793;

class complex_f
{
public:
    complex_f()
    {

    }

    complex_f(float fRe, float fIm)
        :re(fRe)
        ,im(fIm)
    {
    }
    ~complex_f()
    {
    }
public:
    friend complex_f operator + (const complex_f& complex_lv, const complex_f& complex_rv)
    {
        complex_f ret;
        ret.re = complex_lv.re + complex_rv.re;
        ret.im = complex_lv.im + complex_rv.im;
        return ret;
    }
    friend complex_f operator - (const complex_f& complex_lv, const complex_f& complex_rv)
    {
        complex_f ret;
        ret.re = complex_lv.re - complex_rv.re;
        ret.im = complex_lv.im - complex_rv.im;
        return ret;
    }

    friend complex_f operator * (const complex_f& complex_lv, const complex_f& complex_rv)
    {
        complex_f ret;
        ret.re = complex_lv.re*complex_rv.re - complex_lv.im*complex_rv.im;
        ret.im = complex_lv.re*complex_rv.im + complex_lv.im*complex_rv.re;
        return ret;
    }

    friend complex_f operator / (const complex_f& complex_lv, const complex_f& complex_rv)
    {
        complex_f ret;
        double denominator = complex_rv.re * complex_rv.re + complex_rv.im * complex_rv.im;
        ret.re = (complex_lv.re*complex_rv.re + complex_lv.im*complex_rv.im) / denominator;
        ret.im = (complex_lv.im*complex_rv.re - complex_lv.re*complex_rv.im) / denominator;
        return ret;
    }

    complex_f& operator=(const complex_f& comlex_rhl)
    {
        if (this == &comlex_rhl)
            return *this;
        this->re = comlex_rhl.re;
        this->im = comlex_rhl.im;
        return *this;
    }

private:
    float re;
    float im;
};

class FourierTransform
{
    FourierTransform();
    ~FourierTransform();

public:
    bool FFT(complex_f *data, int nCount);

private:
    bool InitFft(int nCount);
    int NumberOfBits(int powerofTwo);
    unsigned BitReverisee(unsigned int x, int log2n);

private:
    int m_nCount;
    int m_nWindow; //´°º¯Êý£¬Ä¬ÈÏººÄþ´°£¨hanning£©
    float *m_pwin;
    complex_f *m_pwt;

public:
    static void kfft(float *pReal, float *pImage, int nCount, int k, float *fr, float *fi, int Inverse, int il);
};

static void kfft(float *pReal, float *pImage, int nCount, int k, float *fr, float *fi, int Inverse, int il);