using System;

namespace DebugLaunch
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] myArgs = {
                "--file",@"E:\vcpkg\installed\x86-windows\include\SDL2\SDL.h",
                "--include",@"E:\vcpkg\installed\x86-windows\include\SDL2",
                "--namespace",@"SDL",
                "--output",@"..\..\..\..\TestSDL\SDL.cs",
                "--libraryPath",@"SDL"
            };
            ClangSharpPInvokeGenerator.Program.Main(myArgs); 
        }
    }
}
