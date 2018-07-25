# colony-simulation
Colony simulation using Neural Nets and Genetic Algorithms

Created with Unity (for linux)

# TODO

non-starred = important<br>
\*           = Important, but do non-starred first<br>
**          = Not really important for now, but needs to be done eventually<br>
***         = Nice to have, not planned to make

- [ ] World generation
    - [ ] Tiles
        - [x] Move spawning of Tiles to the Tile class
        - [x] Tile base class
            - [x] List of AI(s) that are on that tile
        - [x] Tile ground
            - [x] Prefab
            - [x] Class
        - [x] Tile wall
            - [x] Prefab
            - [x] Class
        - [ ] Tile food (*)
            - [ ] Prefab
            - [ ] Class
            - [ ] Grow food over time (**)
- [ ] AI
    - [x] Spawning (basic) 
    - [ ] Behaviour
        - [ ] Basic
            - [ ] Movement
        - [ ] Advanced (Neural nets) (*)
            - [ ] Movement (*)
            - [ ] Eating (**)
            - [ ] Mating (**)
                - [ ] Genetic Algorithm (survival of the fittest) (**)
