# colony-simulation
Colony simulation using Neural Nets and Genetic Algorithms

Created with Unity (for linux)

# TODO

non-starred = important<br>
\*           = Important, but do non-starred first<br>
**          = Not really important for now, but needs to be done eventually<br>
***         = Nice to have, not planned to make

- World generation
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
        - [x] Tile food (*)
            - [x] Prefab
            - [x] Class
            - [x] Food color represents how much food the tile has
            - [ ] Grow food over time (**)
        - [x] Generate food in patches
- Camera
    - [x] Move camera with right mouse button
    - [x] Zoom in/out with scrolling
    - [x] Follow an AI by clicking on it
        - [x] Unfollow by clicking on it again
        - [x] Zoom towards cliked AI
- AI
    - [x] Spawning (basic) 
    - [x] Give Age property
        - [x] Update age every second
    - [ ] Decay dead bodies over time
        - [ ] Maybe ai can decide to go carnivore and eat the dead bodies?
    - [ ] Behaviour
        - [ ] Basic
            - [x] Movement (constant velocity forward)
            - [x] Eating (**)
            - [ ] Reproducing (**)
                - [ ] Genetic Algorithm (**)
                    - Get the genomes of 2 of the best ants and merge/mutate those into genomes for the new generation
                    - The new generation gets added next to the existing generation
        - [x] Advanced (Neural nets) (*)
            - [x] Movement (*)
                - [x] Inputs should be what AI sees (multiple senses that register RGB?)
                - [x] 1 Output, used to rotate the AI