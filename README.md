# zainali?!? - Pengaplikasian Algoritma BFS dan DFS dalam Menyelesaikan Persoalan Maze Treasure Hunt

```
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⠶⣤⣄⣠⢤⣄⣤⢤⣄⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢠⣿⣼⣾⠛⠛⠲⣼⣷⡘⠿⢿⡏⣉⡓⠶⢤⣤⢤⣀⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣴⠿⠾⠟⠁⣤⣄⣀⣀⡀⠙⠓⠚⠷⡽⢿⣷⢀⣰⢿⣫⠿⠭⠟⠳⡆⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣴⠦⣀⠈⣿⣶⡦⢤⣬⣉⣁⣛⣋⣀⣀⡀⠀⣶⣶⣌⠉⡿⠉⣽⣤⡀⠀⠸⣇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⢦⡈⠻⣿⠛⠃⣸⢻⣿⡗⠀⢸⠀⠉⠁⠀⠈⠛⠋⠀⣧⢸⣿⣿⣿⡄⠀⢹⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⢀⣀⣀⡴⠛⠛⠛⠛⠿⠿⣦⣉⣀⣠⡾⠆⠀⠀⠀⠀⠀⠀⠀⣿⠀⠻⣿⡿⠁⠀⣼⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⣴⠚⡹⠴⣶⠟⠛⠛⠛⢳⡶⠲⢤⣙⠣⣅⠓⢩⠀⠀⠀⠀⠀⠀⠀⠻⣄⣀⠀⠰⣶⣦⣿⡄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⢧⡈⠳⠤⠿⠤⠤⠤⠤⠚⠓⠒⠒⠚⠳⠬⠳⡀⢠⣤⣄⠀⠀⠀⠀⢠⠏⣏⠉⠙⠻⢿⣁⡇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠈⣙⣶⡖⠒⠂⠀⠀⠀⠀⠀⠀⠀⠀⠦⢤⠷⠈⠉⠁⠀⠀⠀⠀⠘⣦⠈⣧⡈⠙⠲⢌⡑⢤⡀⠀⠀⠀⠀⠀⠀⠀⠀
⣀⣀⡀⠀⣀⠤⠒⣋⡥⠔⢧⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⡼⠀⠀⠀⠀⠀⠀⠀⠀⠀⣼⠀⢿⣿⡆⠀⠀⡟⠒⢬⡓⢤⡀⠀⠀⠀⠀⠀
⠀⣀⣈⠉⣤⡖⠋⠀⠀⠀⠀⠳⣄⠀⠀⠀⠀⠀⠀⠀⢀⡼⠁⠀⠀⠀⠀⢀⣤⣤⡀⠀⢹⡄⠀⠈⢀⣀⠀⢷⠀⠀⠈⠳⢍⠲⣄⠀⠀⠀
⠉⠀⠈⠙⡇⢹⠀⠀⠀⠀⠀⠀⠈⣹⣶⠤⠤⢤⣤⣶⣯⣤⣄⣀⣀⣀⣀⠈⠻⡿⠋⠀⢰⠇⠀⢀⣈⣿⡧⣼⠀⠀⠀⠀⡠⠽⡞⠓⠢⢤
⠀⠀⠀⠀⠻⠾⠀⠀⠀⠀⠀⠀⠰⣏⣭⡀⢰⣿⣿⣿⣿⣿⣿⠀⠀⠀⠈⠉⠉⣩⣍⠉⢻⡏⢉⣅⡀⠀⠀⢸⡄⠀⠀⡞⣰⠊⠉⠳⡄⢰
⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡠⠔⣫⠵⠛⠙⠒⣾⣿⣿⣿⣿⣿⠦⢤⣀⣀⣀⣠⣿⡋⠀⣰⢃⣈⣛⣁⡤⠤⠼⠃⠀⠀⠉⠉⠀⠀⠀⠸⡌
⠀⠀⠀⣠⣤⠤⠖⠒⠊⠁⡖⠉⠀⠀⠀⠀⠸⣿⣿⣿⣿⣿⣿⠀⠀⠀⠈⠀⠀⠀⠉⠉⠉⠉⠙⢮⠻⣄⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢧
⠀⠀⠀⢿⡿⠦⠔⠒⠒⠚⠁⠀⠀⠀⠀⠀⠀⠀⠀⠉⠉⠉⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⡱⠈⠓⠤⣀⣀⡀⠀⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠓⠦⠤⣄⣀⣨⣿⡷⠀⠀⠀⠀⠀⠀
⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⠛⠀⠀⠀⠀⠀⠀⠀
```
## Lore

> Tuan Krabs menemukan sebuah labirin distorsi terletak tepat di bawah Krusty Krab bernama El Doremi yang Ia yakini mempunyai sejumlah harta karun di dalamnya dan tentu saja Ia ingin mengambil harta karunnya. Dikarenakan labirinnya dapat mengalami distorsi, Tuan Krabs harus terus mengukur ukuran dari labirin tersebut. Oleh karena itu, Tuan Krabs banyak menghabiskan tenaga untuk melakukan hal tersebut sehingga Ia perlu memikirkan bagaimana caranya agar Ia dapat menelusuri labirin ini lalu memperoleh seluruh harta karun dengan mudah. 
>
> Setelah berpikir cukup lama, Tuan Krabs tiba-tiba mengingat bahwa ketika Ia berada pada kelas Strategi Algoritma-nya dulu, Ia ingat bahwa Ia dulu mempelajari algoritma BFS dan DFS sehingga Tuan Krabs menjadi yakin bahwa persoalan ini dapat diselesaikan menggunakan kedua algoritma tersebut. Akan tetapi, dikarenakan sudah lama tidak menyentuh algoritma, Tuan Krabs telah lupa bagaimana cara untuk menyelesaikan persoalan ini dan Tuan Krabs pun kebingungan. Tidak butuh waktu lama, Ia terpikirkan sebuah solusi yang brilian. Solusi tersebut adalah meminta mahasiswa yang saat ini sedang berada pada kelas Strategi Algoritma untuk menyelesaikan permasalahan ini.

## Overview

This project implements a simple maze solver using DFS and BFS algorithm to find a route that visits all checkpoints (treasures) scattered in the maze (and return to the starting point if enabled). 
This project is implemented as a C# Application and is built to meet the following [specifications](https://informatika.stei.itb.ac.id/~rinaldi.munir/Stmik/2022-2023/Tubes2-Stima-2023.pdf).

## Prerequisites

- *Windows* operating system
-  C# `dotnet` installed
- `MSBuild` installed

## Directory Structure

```
├───bin
├───src
│   ├───assets
│   ├───lib
│   ├───obj
│   │   ├───Debug
│   │   │   └───net6.0-windows
│   │   │       ├───ref
│   │   │       ├───refint
│   │   │       └───theme
│   │   └───Release
│   │       └───net6.0-windows
│   │           ├───ref
│   │           ├───refint
│   │           └───theme
│   └───theme
└───test
```

## How To Compile
1. Clone this repository.

```
 $ git clone git@github.com:williamnixon20/Tubes2_zainali.git
 ```

or similarly,

```
 $ git clone https://github.com/williamnixon20/Tubes2_zainali.git
```

2. Open terminal and navigate to the root directory of this repository.
3. Run the following command in terminal.

```
 $ msbuild src/Tubes2_zainali.csproj /p:OutputPath=..\bin\ /p:Configuration=Release
```

4. The resulting executable file will be located at the `bin` directory.
5. \[Troubleshooting\] If the `obj` files in `src` are modified or missing, and are causing compile issues, try navigating to `src` and run
```
 $ dotnet restore
```


## How To Run

1. After [compiling the program](#how-to-compile), navigate to the root directory in terminal.
2. Enter the following command to run the program.

```
 $ .\bin\Tubes2_zainali.exe
```
or similarly,
```
 $ dotnet .\bin\Tubes2_zainali.dll
```
4. Alternatively, navigate to the `bin` folder using *Windows Explorer* and run `Tubes2_zainali.exe`.

## How To Use Program
1. [Compile](#how-to-compile) and [run](#how-to-run) the program.
2. To load custom maze configurations, create `.txt` files containing `K`s, `T`s, `R`s, and `X`s arranged in a rectangular grid. Configuration file syntax can be referred from sample test cases in `test` directory.

>K : Krusty Krab (Start Point), only one start point per maze<br>
>T : Treasure, must have at least one treasure per maze<br>
>R : Walkable maze path<br>
>X : Maze walls<br>

3. Load configuration file using the `File Explorer` button.
4. Press `Visualize` to view the loaded maze.
5. Choose a search mode for traversing the maze by pressing `DFS` or `BFS`. Optionally, enable/disable `Enable TSP` and `Disable Branch Pruning` to toggle TSP mode and Branch Pruning respectively.
6. Press `Search` to search for solution. Next, press `Next` or `Prev` to view search steps or `Autoplay` to animate the search process. 

## Authors

| NIM    | Name                         | GitHub                                            | 
|--------| ---------------------        | ------------------------------------------------- |
|13521074| Eugene Yap Jin Quan          | [yuujin-Q](https://github.com/yuujin-Q)           |
|13521123 | William Nixon                | [williamnixon20](https://github.com/williamnixon20) |
|13521155| Kandida Edgina Gunawan       | [kandidagunawan](https://github.com/kandidagunawan)           |