| Thomas | Cowgod | Params    | Hex  | Args | Description                                                          |
| ------ | ------ | --------- | ---- | ---- | -------------------------------------------------------------------- |
| SYS    | SYS    | nnn       | 0NNN | 1    | System call                                                          |
| CLR    | CLS    |           | 00E0 | 0    | Clear the screen                                                     |
| RTS    | RET    |           | 00EE | 0    | Return from subroutine                                               |
| JUMP   | JP     | nnn       | 1NNN | 1    | Jump to address NNN                                                  |
| CALL   | CALL   | nnn       | 2NNN | 1    | Call routine at address NNN                                          |
| SKE    | SE     | Vx, n     | 3XNN | 2    | Skip next instruction if register X equals NN                        |
| SKNE   | SNE    | Vx, n     | 4XNN | 2    | Do not skip next instruction if register X equals NN                 |
| SKRE   | SE     | Vx, Vy    | 5XY0 | 2    | Skip if register X equals register T                                 |
| LOAD   | LD     | Vx, n     | 6XNN | 2    | Load register X with value NN                                        |
| ADD    | ADD    | Vx, n     | 7XNN | 2    | Add value NN to register X                                           |
| MOVE   | LD     | Vx, Vy    | 8XY0 | 2    | Move value from register X to register Y                             |
| OR     | OR     | Vx, Vy    | 8XY1 | 2    | Perform logical OR on register X and Y and store in Y                |
| AND    | AND    | Vx, Vy    | 8XY2 | 2    | Perform logical AND on register X and Y and store in Y               |
| XOR    | XOR    | Vx, Vy    | 8XY3 | 2    | Perform logical XOR on register X and Y and store in Y               |
| ADDR   | ADD    | Vx, Vy    | 8XY4 | 2    | Add X to Y and store in Y - register F set on carry                  |
| SUB    | SUB    | Vx, Vy    | 8XY5 | 2    | Subtract Y from X and store in X - register F set on !borrow         |
| SHR    | SHR    | Vx, Vy    | 8XY6 | 2    | Shift bits in X 1 bit right, store in Y - bit 0 shifts to register F |
| ??     | SUBN   | Vx, Vy    | 8XY7 | 2    | Subtract X from Y and store in X - register F set on !borrow         |
| SHL    | SHL    | Vx, Vy    | 8XYE | 2    | Shift bits in X 1 bit left, store in Y - bit 7 shifts to register F  |
| SKRNE  | SNE    | Vx, Vy    | 9XY0 | 2    | Skip next instruction if register X not equal register Y             |
| LOADI  | LD     | I, nnn    | ANNN | 1    | Load index with value NNN                                            |
| JUMPI  | JP     | V0, nnn   | BNNN | 1    | Jump to address NNN + index                                          |
| RAND   | RND    | Vx, n     | CYNN | 2    | Generate random number between 0 and NN and store in Y               |
| DRAW   | DRW    | Vx, Vy, z | DXYN | 3    | Draw z byte sprite at x location reg X, y location reg Y             |
| SKPR   | SKP    | Vx        | EX9E | 1    | Skip next instruction if the key in reg X is pressed                 |
| SKUP   | SKNP   | Vx        | EXA1 | 1    | Skip next instruction if the key in reg X is not pressed             |
| MOVED  | LD     | Vx, DT    | FY07 | 1    | Move delay timer value into register Y                               |
| KEYD   | LD     | Vx, K     | FY0A | 1    | Wait for keypress and store in register Y                            |
| LOADD  | LD     | DT, Vx    | FX15 | 1    | Load delay timer with value in register X                            |
| LOADS  | LD     | ST, Vx    | FX18 | 1    | Load sound timer with value in register X                            |
| ADDI   | ADD    | I, Vx     | FX1E | 1    | Add value in register X to index                                     |
| LDSPR  | LD     | F, Vx     | FX29 | 1    | Load index with sprite from register X                               |
| BCD    | LD     | B, Vx     | FX33 | 1    | Store the binary coded decimal value of register X at index          |
| STOR   | LD     | [I], Vx   | FX55 | 1    | Store the values of register X registers at index                    |
| READ   | LD     | Vx, [I]   | FX65 | 1    | Read back the stored values at index into registers                  |
