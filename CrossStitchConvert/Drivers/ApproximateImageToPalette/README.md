# Algorithm design

## Goal

For the following input, find out the cloest approximation of given image:
    - A given palette of colors
    - Number of colors to choose from palette for approximation

"Closest apprxoimation" is defined by the following: we define the distance of approximated image to the original image as `Sum(Manhatten distance of pixel's RGB values)`. Closest approximation is the the approximation with minimal distance for all possible approximations.

## Algorithm flow

Define the following:
- `AllColors` as all colors in palette.
- `AttemptedColors` as the set of colors that have been attempted.
- `PickedColors` as the set of colors picked in approximation so far. 
- `UnattemptedColors` as `AllColors` - `AttemptedColors`.
- `N` as number of colors to pick from palette.

Flow of the algorithm:
1. We will start with one color. Find out the "one color" approximation image. 
2. For each `C` in `UnattemptedColors`:
    - Add it to the existing approximation. Replace pixels with new color where we see the distance using `C` is smaller. 
    - Calculate new image-level distance with newly added `C`. If `C` is not able to reduce distance in any way, add it to `AttemptedColors`. 
3. Pick the `C` with the most distance reduction. Add `C` to `PickedColors` and `AttemptedColors`.
4. Repeat step 2 and 3 until `|PickedColors| = N`

## Data flow

- Define the following:
    - Color Id is the index of the color in palette. 
- We will persist the following intermediate result for each color we add to the approximated image:
    - `[Attempted color Ids]`: all colors that have been attempted. 
    - `[Picked color Ids]`: all colors that have been picked. 
    - `Distance`: distance between current approximated image to original
    - `[[approxmated image in color Ids]]`: two-dimantional array storing the color Ids of approximated image
- The intermatidate result will allow us to do the following:
    - Output to an image file for human judgement
    - Add one more color to produce the next intermediate result