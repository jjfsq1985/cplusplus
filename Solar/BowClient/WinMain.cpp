#include "stdafx.h"
#include "BowClient.h"

int APIENTRY _tWinMain(_In_ HINSTANCE hInstance,
    _In_opt_ HINSTANCE hPrevInstance,
    _In_ LPTSTR    lpCmdLine,
    _In_ int       nCmdShow)
{
    UNREFERENCED_PARAMETER(hPrevInstance);
    UNREFERENCED_PARAMETER(lpCmdLine);

    BowClient ClientFrm(hInstance);
    ClientFrm.Initialize(nCmdShow);
    return ClientFrm.Run();
}
