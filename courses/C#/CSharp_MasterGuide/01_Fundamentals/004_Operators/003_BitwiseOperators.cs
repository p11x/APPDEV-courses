/*
 * ============================================================
 * TOPIC     : C# Fundamentals
 * SUBTOPIC  : Operators - Bitwise Operators
 * FILE      : BitwiseOperators.cs
 * PURPOSE   : This file covers bitwise operators in C#: & (AND), | (OR), ^ (XOR), ~ (NOT), <<, >>.
 *             These operators manipulate individual bits in integral types.
 * ============================================================
 */

// --- SECTION: Bitwise Operators ---
// Bitwise operators perform operations on individual bits of integer values
// Essential for flags, encryption, compression, and low-level operations

using System;

namespace CSharp_MasterGuide._01_Fundamentals._04_Operators
{
    class BitwiseOperators
    {
        static void Main(string[] args)
        {
            // ═══════════════════════════════════════════════════════════════
            // SECTION: Bitwise AND (&)
            // ═══════════════════════════════════════════════════════════════
            
            // Bitwise AND - 1 only if both bits are 1
            // 5 = 0101 (binary)
            // 3 = 0011 (binary)
            // & = 0001 (binary) = 1
            
            int five = 5;
            int three = 3;
            int andResult = five & three;
            
            Console.WriteLine($"5 & 3 = {andResult}"); // Output: 5 & 3 = 1
            // 0101 & 0011 = 0001
            
            // Practical: Check if flag is set
            int flags = 0b1010; // Flag pattern: 10 in decimal
            int flagToCheck = 0b0010; // Check bit 1
            
            bool isFlagSet = (flags & flagToCheck) != 0;
            Console.WriteLine($"Flag set: {isFlagSet}"); // Output: Flag set: True
            
            // Check another flag
            flagToCheck = 0b0100; // Check bit 2
            isFlagSet = (flags & flagToCheck) != 0;
            Console.WriteLine($"Flag 2 set: {isFlagSet}"); // Output: Flag 2 set: False

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Bitwise OR (|)
            // ═══════════════════════════════════════════════════════════════
            
            // Bitwise OR - 1 if either bit is 1
            // 5 = 0101
            // 3 = 0011
            // | = 0111 = 7
            
            int orResult = five | three;
            Console.WriteLine($"5 | 3 = {orResult}"); // Output: 5 | 3 = 7
            // 0101 | 0011 = 0111
            
            // Practical: Set flags
            int fileAttributes = 0;
            fileAttributes |= 0x01; // Set bit 0 (ReadOnly)
            fileAttributes |= 0x02; // Set bit 1 (Hidden)
            fileAttributes |= 0x01; // Already set - stays set
            
            Console.WriteLine($"File attributes: {fileAttributes}"); // Output: File attributes: 3

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Bitwise XOR (^)
            // ═══════════════════════════════════════════════════════════════
            
            // Bitwise XOR - 1 if bits are different
            // 5 = 0101
            // 3 = 0011
            // ^ = 0110 = 6
            
            int xorResult = five ^ three;
            Console.WriteLine($"5 ^ 3 = {xorResult}"); // Output: 5 ^ 3 = 6
            // 0101 ^ 0011 = 0110
            
            // Practical: Toggle flag
            int toggleFlags = 0b1010;
            int toggleMask = 0b0111; // Toggle bits 0,1,2
            
            toggleFlags ^= toggleMask;
            Console.WriteLine($"Toggled: {toggleFlags}"); // Output: Toggled: 14
            
            // XOR swap (without temp variable)
            int x = 5, y = 10;
            x = x ^ y;
            y = x ^ y;
            x = x ^ y;
            Console.WriteLine($"After swap: x={x}, y={y}"); // Output: After swap: x=10, y=5
            
            // XOR encryption (simple example)
            byte key = 0xAA; // 10101010
            byte data = 0x12; // 00010010
            byte encrypted = (byte)(data ^ key);
            byte decrypted = (byte)(encrypted ^ key);
            Console.WriteLine($"Original: {data}, Encrypted: {encrypted}, Decrypted: {decrypted}");
            // Output: Original: 18, Encrypted: 168, Decrypted: 18

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Bitwise NOT (~)
            // ═══════════════════════════════════════════════════════════════
            
            // Bitwise NOT - flips all bits (ones complement)
            // 5 = 0000000000000101
            // ~5 = 1111111111111010 = -6 (two's complement)
            
            int notResult = ~5;
            Console.WriteLine($"~5 = {notResult}"); // Output: ~5 = -6
            
            // Practical: Create bit mask for all bits set
            int allBitsSet = ~0;
            Console.WriteLine($"~0 = {allBitsSet}"); // Output: ~0 = -1
            
            // Get complement (extract upper bits)
            int value = 0b11110000;
            int complement = ~value & 0b1111; // Get lower 4 bits
            Console.WriteLine($"Complement: {complement}"); // Output: Comlement: 15

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Left Shift (<<)
            // ═══════════════════════════════════════════════════════════════
            
            // Left shift - shifts bits left, zeros fill right
            // 1 << n = multiply by 2^n
            
            int shiftLeft = 1 << 3; // 1 * 2^3 = 8
            Console.WriteLine($"1 << 3 = {shiftLeft}"); // Output: 1 << 3 = 8
            
            int shiftLeft2 = 5 << 2; // 5 * 4 = 20
            Console.WriteLine($"5 << 2 = {shiftLeft2}"); // Output: 5 << 2 = 20
            // 0101 << 2 = 10100 = 20
            
            // Practical: Create power-of-2 values
            for (int i = 0; i < 8; i++)
            {
                Console.WriteLine($"2^{i} = {1 << i}"); // Output: 1, 2, 4, 8, 16, 32, 64, 128
            }

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Right Shift (>>)
            // ═══════════════════════════════════════════════════════════════
            
            // Right shift - shifts bits right
            // For signed types: arithmetic shift (sign bit fills)
            // For unsigned: logical shift (zero fills)
            
            int shiftRight = 16 >> 2; // 16 / 4 = 4
            Console.WriteLine($"16 >> 2 = {shiftRight}"); // Output: 16 >> 2 = 4
            
            int shiftRight2 = 17 >> 2; // 17 / 4 = 4 (truncates)
            Console.WriteLine($"17 >> 2 = {shiftRight2}"); // Output: 17 >> 2 = 4
            
            // Negative numbers (arithmetic shift)
            int negative = -8;
            int negativeShift = negative >> 1;
            Console.WriteLine($"-8 >> 1 = {negativeShift}"); // Output: -8 >> 1 = -4
            
            // Unsigned right shift (C# 2020 / .NET 5+)
            uint unsignedVal = 0xFFFFFFFF;
            uint logicalShift = unsignedVal >>> 16;
            Console.WriteLine($"0xFFFFFFFF >>> 16 = {logicalShift}"); // Output: 0xFFFFFFFF >>> 16 = 65535

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Compound Bitwise Assignments
            // ═══════════════════════════════════════════════════════════════
            
            int val = 5;
            val &= 3; // val = val & 3
            Console.WriteLine($"5 &= 3: {val}"); // Output: 5 &= 3: 1
            
            val = 5;
            val |= 3; // val = val | 3
            Console.WriteLine($"5 |= 3: {val}"); // Output: 5 |= 3: 7
            
            val = 5;
            val ^= 3; // val = val ^ 3
            Console.WriteLine($"5 ^= 3: {val}"); // Output: 5 ^= 3: 6
            
            val = 8;
            val <<= 2; // val = val << 2
            Console.WriteLine($"8 <<= 2: {val}"); // Output: 8 <<= 2: 32
            
            val = 16;
            val >>= 2; // val = val >> 2
            Console.WriteLine($"16 >>= 2: {val}"); // Output: 16 >>= 2: 4

            // ═══════════════════════════════════════════════════════════════
            // SECTION: Real-World Examples
            // ═══════════════════════════════════════════════════════════════
            
            // ── File attributes as flags ───────────────────────────────────
            [Flags] // Enables nice ToString for flags
            enum FileAttributes2
            {
                None = 0,
                ReadOnly = 1,
                Hidden = 2,
                System = 4,
                Directory = 16,
                Archive = 32
            }
            
            FileAttributes2 attrs = FileAttributes2.ReadOnly | FileAttributes2.Hidden;
            Console.WriteLine($"Flags: {attrs}"); // Output: Flags: ReadOnly, Hidden
            
            bool isReadOnly = (attrs & FileAttributes2.ReadOnly) != 0;
            bool isHidden = (attrs & FileAttributes2.Hidden) != 0;
            Console.WriteLine($"Is ReadOnly: {isReadOnly}, Hidden: {isHidden}");
            // Output: Is ReadOnly: True, Hidden: True
            
            // ── Bit manipulation for performance ─────────────────────────
            // Checking odd/even using bitwise
            int num = 7;
            bool isOdd = (num & 1) == 1;
            Console.WriteLine($"{num} is odd: {isOdd}"); // Output: 7 is odd: True
            
            // ── Fast division/multiplication by powers of 2 ───────────────
            int fastMult = 10 << 3; // 10 * 8 = 80
            int fastDiv = 80 >> 2;  // 80 / 4 = 20
            Console.WriteLine($"10 << 3 = {fastMult}, 80 >> 2 = {fastDiv}");
            // Output: 10 << 3 = 80, 80 >> 2 = 20
        }
    }
}
