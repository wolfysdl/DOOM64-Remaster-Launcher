﻿/*  A DOOM64 Remaster Launcher
    Copyright (C) 2023 Gibbon

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Xml.Linq;
using DOOM64_Launcher;

namespace GameLauncher
{
    public static class KPFBackupButtonListener
    {
        public static void backupOriginalKPFButton_Click()
        {
            // Copy the kpf file and rename it
            if (File.Exists(GlobalDeclarations.DOOM64KPF))
            {
                string filePath = GlobalDeclarations.DOOM64KPF;
                string expectedHash = "A1113F6B5F878AC90961D438A70FBB0A3ECB60C071A6C1F0C7867A0E5AD43F32";

                using (var sha256 = SHA256.Create())
                {
                    using (var stream = File.OpenRead(filePath))
                    {
                        byte[] computedHash = sha256.ComputeHash(stream);

                        // Convert the expected hash from hex to byte
                        byte[] expectedHashBytes = new byte[expectedHash.Length / 2];
                        for (int i = 0; i < expectedHashBytes.Length; i++)
                        {
                            expectedHashBytes[i] = Convert.ToByte(expectedHash.Substring(i * 2, 2), 16);
                        }

                        // Compare the hash
                        if (StructuralComparisons.StructuralEqualityComparer.Equals(computedHash, expectedHashBytes))
                        {
                            if (File.Exists(GlobalDeclarations.DOOM64KPFORIG))
                            {
                                MessageBox.Show("INFORMATION: Doom64.kpf backup already exists",
                                                "KPF Backup",
                                                 MessageBoxButtons.OK,
                                                 MessageBoxIcon.Information);
                            } else {
                                File.Copy(GlobalDeclarations.DOOM64KPF, GlobalDeclarations.DOOM64KPFORIG);
                            }

                            if (File.Exists(GlobalDeclarations.DOOM64KPFORIG))
                            {
                                MessageBox.Show("SUCCESS: Doom64.kpf has been backed up.",
                                    "KPF Backup",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("ERROR: Doom64.kpf hash does not match the expected hash.",
                                    "KPF Hash Error",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
    }
}
