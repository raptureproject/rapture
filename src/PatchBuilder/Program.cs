// Licensed to the Rapture Project under one or more agreements.
// The Rapture Project licenses this file to you under the MIT license.

using Rapture.PatchBuilder.ZiPatch;
using System.Text;

using var bootPatch = new Patch(2010, 9, 18, 1, PatchType.Boot);

var bootFile = bootPatch.AddFile("ffxivboot.exe");

// SetProcessAffinityMask
bootFile.ApplyPatch(0x3D7B, [.. Enumerable.Repeat<byte>(0x90, 30)]);

// SQEX::CDev::Engine::Vfx::Qix::Thread::ThreadManager::CreateEffectThread
bootFile.ApplyPatch(0x70056B, [0xB5, 0x01]);
bootFile.ApplyPatch(0x700613, [0xB5, 0x01]);

// rsa_verify
bootFile.ApplyPatch(0x64324, [0x01, 0x00, 0x00, 0x00]);

// ver01.ffxiv.com -> 127.0.0.1
bootFile.ApplyPatch(0x966404, Encoding.ASCII.GetBytes("127.0.0.1\0\0\0\0\0\0\0"));

bootPatch.Save();
