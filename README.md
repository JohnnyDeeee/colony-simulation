# Rebuilding this project here: https://github.com/JohnnyDeeee/Improved-Colony-Simulation

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
    - [ ] Move some stuff from AI to AIAnt that dont belong in AI
    - [ ] Save best AI weights
        - [ ] Load AI weights
    - [ ] Decay dead bodies over time (**)
        - [ ] Maybe ai can decide to go carnivore and eat the dead bodies? (**)
    - [ ] Behaviour
        - [ ] Basic
            - [x] Movement (constant velocity forward)
            - [x] Eating (**)
            - [ ] Reproducing
                - [x] Genetic Algorithm
                    - Get the genomes of 2 of the best ants and merge/mutate those into genomes for the new generation
                    - The new generation gets added next to the existing generation
                    - Use ai's age as fitness
                - [ ] Actually let 2 ants produce a baby (not just create a whole new population)
            - [ ] Sight
                - [x] Eye(s) register RGB
                - [ ] Choose hits randomly? Now we just pick the first X ones (where X = this.eyeAmount)
                - [ ] Make raycast for each eye ?
        - [x] Advanced (Neural nets) (*)
            - [x] Movement (*)
                - [x] Inputs should be what AI sees (multiple senses that register RGB?)
                - [x] 1 Output, used to rotate the AI
                - [ ] Ants don't really seem to improve ...
- UI
    - [ ] World info
        - [ ] Split generation info into its own class
        - [x] Basic info
        - [ ] BUG "Highest ant age" is not highest at all times.. (now shows highest from last population)
    - [ ] Add fitness values into a continues diagram?
    - [ ] Selected AI info
        - [x] AI info
        - [x] Vision (show the color it is seeing?)
        - [x] AIAnt info
        - [ ] Show ai in seperate camera (so dont follow ai with main camera when selected) (**)
    - [x] Speed buttons
