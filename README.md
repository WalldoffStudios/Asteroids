# Asteroids
![Gameplay](Assets/Asteroids/Giphy/AsteroidsGameplay.gif "Asteroids")


**Code design: **
I aimed to adhere strictly to the SOLID principles in designing the project, utilizing Extenject (Zenject) to facilitate this adherence. I am also a strong proponent of the KISS principle, striving to implement it as effectively as possible.

The game initializes from a boot scene where I bind the GameManager and signals within the project context, in addition to loading and binding the addressables from Unity Cloud. I was not entirely satisfied with how I currently bind the addressables by accessing the project context's DiContainer, but time constraints prevented me from coming up with a better approach.

In the game scene I use Factories and memory pools for managing dynamic elements such as asteroids, bullets, and UICoins. With an eye toward future collaboration, I organized the UI components as prefabs that are instantiated during the binding process. This setup wasn't just about streamlining my workflow—it was also about laying the groundwork for seamless teamwork with designers. By preparing these prefabs, I aimed to create an environment where, should the opportunity for collaboration arise, designers could easily dive in and contribute, working on these elements independently.

The game operates on a state-driven model, with simple events triggered by the GameManager. Currently, when the player dies, the game scene simply reloads.

The player is decoupled with several different classes each handling a specific topic in order to adhere to the single responsibility principle. 

**Decisions and challenges:**
- Opting for Extenject (Zenject) was a deliberate choice despite my lack of prior experience with it. The initial learning phase was somewhat slow, yet as I grew more familiar with its features, my appreciation for it deepened. It has reached a point where I envision using it for all my personal projects going forward. Although the learning curve initially slowed my progress, the investment has proven worthwhile.

- Unit Tests: Unfortunately, I did not incorporate unit tests in my project. However, after researching the topic extensively, I recognize their value and plan to integrate them into my future projects regularly.

- Addressables: Having previous experience with addressables, I was able to implement them relatively quickly. Currently, I am storing three assets with addressables but only utilize two, as I did not prioritize integrating audio into the game.

- Visual effects: As I've worked quite a lot with shaders I had quite a few ideas on cool effects to implement but I ended up not writing any shader code at all, I dont even have a flashing effect on the player when it takes damage. I felt like it was more important to focus solely on a clean codebase than making the game look good and I'm somewhat happy about the result in the time given.

