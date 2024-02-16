# EffortAndDistanceOptimizer

Consider a row of urinals in a public bathroom.
```math
\begin{bmatrix}
 0  1  2  3  4  5 \\ 
 0  0  0  0  0  0 
\end{bmatrix}
```
The top row represents the index of the urinal, and the bottom row represents if a person is standing there. Hence, the above stalls in empty. In this scenario, it is obvious that the ideal position is index 0. Because you are not next to anyone, and it is the shortest distance to walk from the 1st index of the collection.

The objective is to be as far as possible from other people (_c_ for **c**loseness), but also not walk too far (_f_ for e**f**fort).

Here we have another exmple, where one space is occupied upon arrival. There may be zero or more occupied spaces upon the arrival of a new person.
```math
\begin{bmatrix}
 0  1  2  3  4  5 \\ 
 1  0  0  0  0  0 
\end{bmatrix}
```
Here, a good spot to stand, may be index 2, because there is a space between the new person and the one other person. The new collection would look like this:
```math
\begin{bmatrix}
 0  1  2  3  4  5 \\ 
 1  0  1  0  0  0 
\end{bmatrix}
```
But how do we define this need, and how do we generalize this? 

My solution is an algorith, that produduces a badness _b_ score for each index, then inserts the new person, based on the minimum badness score. The badness score: _b_ =  _f_ + _c_

_f_ is simple, as it is just the index of the collection, with a weight attached that defaults to 1 _d_:
```math
f = i\cdot d
```
_c_ is more complex, as it is an exponential decay function, also with a weight _w_ that defaults to 7:
```math
c = w\cdot e^{-2x}
```
_c_ with a weight of 7 illustrated:


<img width="378" alt="image" src="https://github.com/DivanVanZyl/EffortAndDistanceOptimizer/assets/5897077/b42aaa78-ecf6-4ddc-84ca-65a29abde4de">

The reason for the exponential decay, is that it is a lot beter to stand one space away from someone else at the urinal, but it is only marginally better to stand 2 spaces away from someone than 1 space away. And the difference between two spaces and 3 is insignificant. But there is a weight _w_ that can be adjusted for this.
