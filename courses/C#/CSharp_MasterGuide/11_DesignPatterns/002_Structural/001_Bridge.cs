/*
 * ============================================================
 * TOPIC     : Design Patterns
 * SUBTOPIC  : Structural - Bridge Pattern
 * FILE      : 03_Bridge.cs
 * PURPOSE   : Demonstrates Bridge design pattern in C#
 * ============================================================
 */
using System; // needed for Console, basic types

namespace CSharp_MasterGuide._11_DesignPatterns._02_Structural
{
    /// <summary>
    /// Demonstrates Bridge pattern
    /// </summary>
    public class BridgePattern
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Bridge Pattern ===\n");

            Console.WriteLine("1. Bridge - Decouple Abstraction from Implementation:");
            var remote = new AdvancedRemote(new TV());
            remote.Power();
            remote.VolumeUp();
            // Output: TV powered on
            // Output: Volume increased

            Console.WriteLine("\n2. Multiple Implementations:");
            var radioRemote = new BasicRemote(new Radio());
            radioRemote.Power();
            // Output: Radio powered on

            Console.WriteLine("\n=== Bridge Complete ===");
        }
    }

    public interface IDevice
    {
        void PowerOn(); void PowerOff(); void SetVolume(int level);
    }

    public class TV : IDevice
    {
        public void PowerOn() => Console.WriteLine("   TV powered on");
        public void PowerOff() => Console.WriteLine("   TV powered off");
        public void SetVolume(int level) => Console.WriteLine($"   Volume increased");
    }

    public class Radio : IDevice
    {
        public void PowerOn() => Console.WriteLine("   Radio powered on");
        public void PowerOff() => Console.WriteLine("   Radio powered off");
        public void SetVolume(int level) => Console.WriteLine($"   Radio volume up");
    }

    public abstract class RemoteControl
    {
        protected IDevice _device;
        protected RemoteControl(IDevice device) => _device = device;
        public abstract void Power();
    }

    public class BasicRemote : RemoteControl
    {
        public BasicRemote(IDevice device) : base(device) { }
        public override void Power() => _device.PowerOn();
    }

    public class AdvancedRemote : RemoteControl
    {
        public AdvancedRemote(IDevice device) : base(device) { }
        public override void Power() => _device.PowerOn();
        public void VolumeUp() => _device.SetVolume(10);
    }
}