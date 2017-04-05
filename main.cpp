/*
* Author: Cornelius Bott
* Brief:  This program recursively updates Files in a folder
*/
#include <stdio.h>
#include <stdint.h>
#include <string>
#include <iostream>
#include <Windows.h>

using namespace std;

int dir_scan()
{

	return 0;
}


int main(int argc, char *argv[], char *envp[])
{
	string startpath, targetpath;
	HANDLE hFind = INVALID_HANDLE_VALUE;
	WIN32_FIND_DATA ffd;
	TCHAR szDir[MAX_PATH];
	DWORD dwError = 0;
	//read command-line parameters
	if (argc < 3) {
		cout << "To few arguments" << endl;
		cout << "Usage: simplesync <startpath> <targetpath> [options]" << endl;
	}
	else {
		startpath = argv[1];
		targetpath = argv[2];
	}
	//start with scanning
	cout << "Start Path is: " << startpath << endl;
	cout << "Target Path is: " << targetpath << endl;

	hFind = FindFirstFile(argv[1], &ffd);

	if (INVALID_HANDLE_VALUE == hFind)
	{
		//DisplayErrorBox(TEXT("FindFirstFile"));
		return dwError;
	}

	while (FindNextFile(hFind, &ffd) != 0);
	return 0;
}