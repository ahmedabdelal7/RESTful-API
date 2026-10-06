using System;
using NAudio.CoreAudioApi;

namespace AudioAPI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Volume Control App");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1: Increase Volume");
            Console.WriteLine("2: Decrease Volume");
            Console.WriteLine("3: Set Volume (0-100)");
            Console.WriteLine("4: Exit");

            var deviceEnumerator = new MMDeviceEnumerator();
            var device = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

            bool running = true;
            while (running)
            {
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        device.AudioEndpointVolume.VolumeStepUp();
                        Console.WriteLine($"Volume increased → Now: {(int)(device.AudioEndpointVolume.MasterVolumeLevelScalar * 100)}%");
                        break;
                    case "2":
                        device.AudioEndpointVolume.VolumeStepDown();
                        Console.WriteLine($"Volume decreased → Now: {(int)(device.AudioEndpointVolume.MasterVolumeLevelScalar * 100)}%");
                        break;
                    case "3":
                        Console.Write("Enter volume (0-100): ");
                        if (int.TryParse(Console.ReadLine(), out int vol))
                        {
                            device.AudioEndpointVolume.MasterVolumeLevelScalar = vol / 100.0f;
                            Console.WriteLine($"Volume set to {vol}%.");
                        }
                        break;
                    case "4":
                        running = false;
                        break;
                }
            }
        }
    }
}