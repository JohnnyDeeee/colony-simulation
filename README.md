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
        - [x] Convert tile classes to MonoBehaviour scripts and use Unity as it should be used
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
- [x] Camera
    - [x] Move camera with right mouse button
    - [x] Zoom in/out with scrolling
    - [x] Follow an AI by clicking on it
        - [x] Unfollow by clicking on it again
        - [x] Zoom towards cliked AI
- [ ] AI
    - [x] Spawning (basic) 
    - [ ] Behaviour
        - [ ] Basic
            - [ ] Movement
        - [ ] Advanced (Neural nets) (*)
            - [ ] Movement (*)
                - [ ] Inputs should be what AI sees (multiple senses that register RGB?)
                - [ ] 2 Outputs that are angles to which the AI steers (so always moving forwards)
            - [ ] Eating (**)
            - [ ] Mating (**)
                - [ ] Genetic Algorithm (survival of the fittest) (**)
