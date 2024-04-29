# CHIP-8 Challenge
This is the source code for the multi-week mob programming challenge for the RMG Code Dojo beginning **26/02/24**.

You should be able to pick up the changes from each session from this repo before the next session. Each week, the coder driving will submit a PR containing that week's changes, which I will review and accept. If the coder driving does not have Github access, I will do this for them. 

## Challenge details

The goal of this challenge is to write a functioning CHIP-8 Virtual Machine and emulator capable of executing programs written in the CHIP-8 language.

The specific goal is to complete a VM environment that can run the game of Pong found here: https://github.com/badlogic/chip8/blob/master/roms/sources/PONG.SRC

## Challenge rules

1. Please don't take code from existing CHIP-8 emulators on Github (including mine!). The fun of the challenge is in doing it ourselves. That said, if we do decide we need guidance you can of course look at other implementations or even ask ChatGPT to help you out!

2. Don't write code outside of the Code Dojo sessions - it's tempting to work on stuff yourself in your own time, but this short-changes the group who can learn from the experience of thinking about these problems together.

3. Do bring ideas to the session - there's nothing to stop you researching the problem on your own time, and bringing your thoughts to the group; but the group gets the final say on what we do.

## Resources

This list will grow as we go, but here are some useful resources to get started with:

https://en.wikipedia.org/wiki/CHIP-8 - the Wikipedia article on CHIP-8 (lists opcodes etc)

[OPCODES.md](https://github.com/rmgdojo/CodeDojos/blob/main/Chip-8%20Challenge/src/OPCODES.md)- Opcode list with both opcode 'dialects' for conversion

https://github.com/badlogic/chip8/tree/master/roms/sources - some CHIP-8 games from badlogic's Kotlin implementation

https://github.com/badlogic/chip8/ - CHIP-8 in Kotlin

https://www.pong-story.com/chip8/ - David Winter's CHIP-8 Emulator page (which started it all)

https://www.emutalk.net/threads/chip-8.19894/ - useful (but OLD) thread on EmuTalk about CHIP-8 emulation

https://github.com/craigthomas/Chip8Assembler - CHIP 8 assembler project (useful for list of mnemonics)

http://devernay.free.fr/hacks/chip8/C8TECH10.HTM#00EE - Detail on what each instruction does (note this uses different mnemonics to Craig Thomas's list)
