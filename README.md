# Golf It!

Golf It! is a 2D, side-view mini-golf prototype built in Unity for the
Web3Task Game Developer Intern technical assignment. The core mechanic
is a physics-driven golf shot: the player adjusts the launch angle,
charges power, and releases the swing to launch the ball across a curved
course.

## Gameplay

The game contains five progressively varied courses:

1.  **Gentle Bend** -- introduces the curved track.
2.  **Switchback** -- uses an S-shaped course.
3.  **Double Dip** -- combines two dips and rises.
4.  **Narrow Passage** -- features a tighter elevated section and
    descent.
5.  **Final Putt** -- combines multiple curves for the final challenge.

The player succeeds by guiding the ball into the goal. A shot can fail
if the ball misses the course or falls below the play area. Players can
retry a level, return to the main menu, or progress to the next level
after completing a course.

## Controls


### Android

-   **Up arrow button:** Adjust aim in upwards direction.
-   **Down arrow button:** Adjust aim in downwards direction.
-   **Swing button:** Hold to charge; release to launch.

The on-screen controls are positioned on the game canvas and use Unity's
Input System.

## Implementation

### Launch and aiming

The launch controller reads input actions for aiming and swinging. The
aim angle is adjusted over time and clamped between configured minimum
and maximum values. Holding the swing input increases the charge amount.
On release, the charge amount is mapped to a launch power, and an
impulse is applied to the ball's `Rigidbody2D` in the selected
direction.

A club pivot visually moves during the backswing and forward swing. A
dotted aiming indicator displays the current launch direction before the
shot. The indicator is hidden after launch.

### Curved surface interaction

The ball uses a `Rigidbody2D` and `CircleCollider2D`. Each course uses
an `EdgeCollider2D` to define its curved path. A `LineRenderer` draws
the visible course, using points corresponding to the collider path.

The ball's movement along or away from the course is handled through
Unity 2D physics and collisions. Launch angle and applied impulse affect
how the ball contacts and travels across the surface. The courses use
different path shapes to vary the challenge.

### Goal, failure and progression

A trigger collider in the goal area detects the player ball and reports
completion to the game manager. A fall detector checks whether the ball
has dropped below the play area and reports failure. The game manager
prevents repeated result handling, displays the appropriate result
panel, and provides retry, menu and level-progression actions.

### Feedback

-   A `Trail Renderer` follows the moving ball.
-   A particle effect plays at collision contact points.
-   A swing sound plays when the club reaches the launch point.
-   Separate sounds play for level completion and failure.
-   Android vibration feedback is used for shooting and failure.

## Project setup

-   **Engine:** Unity 6.3 LTS
-   **Project type:** 2D
-   **Input:** Unity Input System
-   **Target platform:** Android
-   **Scenes:** `MainMenu`, `Level_01`, `Level_02`, `Level_03`,
    `Level_04`, `Level_05`

To open the project, use Unity Hub to add the project folder and open it
with a compatible Unity 6.3 LTS installation. Allow Unity to finish
importing assets before opening the scenes or building.

For an Android build, select the Android Build Profile and ensure the
listed scenes are included in the build.

## Challenges and solutions

-   **Keeping visuals aligned with collision geometry:** The visible
    course and its `EdgeCollider2D` need matching point positions. The
    same path coordinates are used so the ball does not appear to travel
    on a different curve from the one it collides with.
-   **Getting predictable shots:** Angle and charge both affect the
    launch. Course layouts and goal placement were tested and adjusted
    through repeated shots rather than relying on a single successful
    attempt.
-   **Avoiding unstable ball feedback:** Visual feedback should not
    interfere with the ball's physics collider. The ball's transform is
    therefore kept separate from visual effects that could alter its
    collision shape.
-   **Supporting desktop and mobile input:** The same gameplay actions
    are connected to keyboard controls and on-screen mobile controls
    through the Input System.

## Possible improvements

Given more development time, I would: - Add more course variations and
optional difficulty settings. - Improve shot feedback with a more
detailed trajectory preview. - Add scoring based on shot count and
completion time. - Tune physics and control sensitivity across a wider
range of Android devices. - Add additional visual polish, accessibility
options and audio settings.

## Notes

This project was developed as a focused prototype for the supplied
technical assignment. The implementation prioritizes the launch
mechanic, physics interaction, clear outcomes, and a working mobile
build over a larger feature set.
