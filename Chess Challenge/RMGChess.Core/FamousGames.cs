using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMGChess.Core
{
    public static class FamousGames
    {
        public static Dictionary<string, string[]> Games = new Dictionary<string, string[]>
        {
            { "Game of the Century (Byrne vs. Fischer, 1956)", new string[]
                {
                    "Nf3", "Nf6", "c4", "g6", "Nc3", "Bg7", "d4", "O-O", "Bf4", "d5", "Qb3", "dxc4", "Qxc4", "c6", "e4", "Nbd7",
                    "Rd1", "Nb6", "Qc5", "Bg4", "Bg5", "Na4", "Qa3", "Nxc3", "bxc3", "Nxe4", "Bxe7", "Qb6", "Bc4", "Nxc3", "Bc5",
                    "Rfe8+", "Kf1", "Be6", "Bxb6", "Bxc4+", "Kg1", "Ne2+", "Kf1", "Nxd4+", "Kg1", "Ne2+", "Kf1", "Nc3+", "Kg1",
                    "axb6", "Qb4", "Ra4", "Qxb6", "Nxd1", "h3", "Rxa2", "Kh2", "Nxf2", "Re1", "Rxe1", "Qd8+", "Bf8", "Nxe1", "Bd5",
                    "Nf3", "Ne4", "Qb8", "b5", "h4", "h5", "Ne5", "Kg7", "Kg1", "Bc5+", "Kf1", "Ng3+", "Ke1", "Bb4+", "Kd1", "Bb3#"
                }
            },
            { "Opera Game (Morphy vs. Duke & Count, 1858)", new string[]
                {
                    "e4", "e5", "Nf3", "d6", "d4", "Bg4", "dxe5", "Bxf3", "Qxf3", "dxe5", "Bc4", "Nf6", "Qb3", "Qe7", "Nc3",
                    "c6", "Bg5", "b5", "Nxb5", "cxb5", "Bxb5+", "Nbd7", "O-O-O", "Rd8", "Rxd7", "Rxd7", "Rd1", "Qe6", "Bxd7+",
                    "Nxd7", "Qb8+", "Nxb8", "Rd8#"
                }
            },
            { "Immortal Game (Anderssen vs. Kieseritzky, 1851)", new string[]
                {
                    "e4", "e5", "f4", "exf4", "Bc4", "Qh4+", "Kf1", "b5", "Bxb5", "Nf6", "Nf3", "Qh6", "d3", "Nh5", "Nh4", "Qg5",
                    "Nf5", "c6", "g4", "Nf6", "Rg1", "cxb5", "h4", "Qg6", "h5", "Qg5", "Qf3", "Ng8", "Bxf4", "Qf6", "Nc3", "Bc5",
                    "Nd5", "Qxb2", "Bd6", "Bxg1", "e5", "Qxa1+", "Ke2", "Na6", "Nxg7+", "Kd8", "Qf6+", "Nxf6", "Be7#"
                }
            },
            //{ "Evergreen Game (Anderssen vs. Dufresne, 1852)", new string[]
            //    {
            //        "e4", "e5", "f4", "exf4", "Nf3", "g5", "h4", "g4", "Ng5", "h6", "Nxf7", "Kxf7", "d4", "d5", "Bxf4", "Nf6",
            //        "Nc3", "Bb4", "e5", "Ne4", "Qd3", "c5", "O-O-O", "Bxc3", "bxc3", "Qa5", "Kc2", "c4", "Qe3", "Qa4+", "Kb1",
            //        "Bf5", "Be2", "Nxc3+", "Qxc3", "Na6", "a3", "Rb8+", "Ka1", "Rb3", "Qc1", "Rxa3+", "Qxa3", "Qxa3#"
            //    }
            //},
            { "Deep Blue vs. Kasparov (1997, Game 6)", new string[]
                { 
                  "e4", "c6", "d4", "d5", "Nc3", "dxe4", "Nxe4", "Nd7", "Nf3", "Ngf6", "Nxf6+", "Nxf6", "h3", "Bf5", "Bd3",
                  "Bxd3", "Qxd3", "e6", "O-O", "Be7", "Re1", "O-O", "c4", "Qa5", "Bd2", "Qa6", "a4", "Rad8", "Bc3", "c5",
                  "d5", "Nxd5", "Qe4", "Nxc3", "bxc3", "Bf6", "Re3", "Rd6", "a5", "Qc6", "Qf4", "Rfd8", "Ne5", "Qc7",
                  "Ng4", "Rd1+", "Rxd1", "Rxd1+", "Kh2", "Rh1+", "Kxh1", "Qxf4", "gxf4", "Rd3", "b6", "axb6",
                  "axb6", "Rxb6", "Bxc3", "Rb8+", "Kg7", "Nh2", "Be1", "Nf3", "Bxf2", "Rb7", "Kf6", "Rc7", "e5", "c5",
                  "e4", "Nd2", "Ke5", "Nc4+", "Kd5", "Na5", "e3", "Nb3", "e2", "Rd7+", "Kc4", "Nd2+", "Kc3", "Nf3",
                  "e1=Q", "Nxe1", "Bxe1", "c6", "Bg3", "c7", "f3", "c8=Q", "f2", "Qc4+", "Kd2", "Qd3#" 
                }
            }
        };
    }
}
