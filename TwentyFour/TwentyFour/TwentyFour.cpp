// TwentyFour.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"

#include <string>
#include <iostream>
#include <cmath>
using namespace std;

#define N (4)
#define EPS (1e-9)
double d[N] = {2, 6, 3, 4};
string expr[N];

int cmp(const double &a, const double &b)
{
	if (fabs(a - b) < EPS) return 0;
	if(a - b > 0) return 1;
	return -1;
}

void dfs(int n)
{
	double a, b;
	string expa, expb;
	if(n == 1)
	{
		if(cmp(d[0], 24) == 0)
		{
			cout << expr[0] << " = " << d[0] << endl;
		}
		return;
	}
	int i, j;
	for (i = 0; i < n; ++i)
	{
		for (j = i + 1; j < n; ++j)
		{
			a = d[i];
			b = d[j];
			d[j] = d[n-1];
			expa = expr[i];
			expb = expr[j];
			expr[j] = expr[n-1];

			//a + b
			d[i] = a + b;
			expr[i] = "(" + expa + "+" + expb + ")";
			dfs(n - 1);
	
			if(cmp(a,b) >= 0)//正数
			{
				//a - b
				d[i] = a - b;
				expr[i] = "(" + expa + "-" + expb + ")";
				dfs(n - 1);
			}
			else
			{
				//b - a
				d[i] = b - a;
				expr[i] = "(" + expb + "-" + expa + ")";
				dfs(n - 1);
			}

			//a * b
			d[i] = a * b;
			expr[i] = "(" + expa + "*" + expb + ")";
			dfs(n - 1);

			//a / b
			if(cmp(b, 0) != 0)
			{
				if(cmp(a/b,1.0*int(a/b)) == 0)//整数
				{
					d[i] = a / b;
					expr[i] = "(" + expa + "/" + expb + ")";
					dfs(n - 1);
				}
			}
			//b / a
			if(cmp(a, 0) != 0)
			{
				if(cmp(b/a,1.0*int(b/a)) == 0)//整数
				{
					d[i] = b / a;
					expr[i] = "(" + expb + "/" + expa + ")";
					dfs(n - 1);
				}
			}

			//回溯
			expr[j] = expb;
			expr[i] = expa;
			d[j] = b;
			d[i] = a;
		}
	}
}

int _tmain(int argc, _TCHAR* argv[])
{

	for (int i = 0; i < N; ++i){
		expr[i] = (int)d[i] + '0';
	}
	dfs(N);
	return 0;
}

