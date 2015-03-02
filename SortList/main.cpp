#include <iostream>

using namespace std;

#include "SwapObject.h"
#include "SList.h"

void swapInt(int& left, int& right);

int main()
{
    int a = 45;
    int b = 23;

    cout << "before a:"<< a << "; b:" << b<<endl;

    swapInt(a,b);

    cout << "after a:"<< a << "; b:" << b<<endl;

int *pData =new int[10];
for(int i=0; i< 10; i++)
    pData[i]= i+1;
    SwapObject aObj;
    aObj.SetData(10,pData);
    delete[] pData;

    int *pSwap =new int[20];
for(int i=0; i< 20; i++)
    pSwap[i]= 10+i+1;
    SwapObject bObj;
    bObj.SetData(20,pSwap);
    delete[] pSwap;

    cout << "before a:"<< aObj.Print() << "\nb:" << bObj.Print()<<endl;

    SwapObject::SwapContent(aObj,bObj);

    cout << "before a:"<< aObj.Print() << "\nb:" << bObj.Print()<<endl;


    SList *pTestList = new SList();
    pTestList->InitSList(5);
    pTestList->ReleaseSList();
    delete pTestList;



    return 0;
}

void swapInt(int& left, int& right)
{
    left ^= right;
    right ^= left;
    left ^= right;
}
