# NAKISSOS3D

INTRO
Our project consists of a mirror in which people would see a reflection of themselves, but in
an ancient city with ancient clothes. This project aims to bring back the interest of the new
generation to history, as people go less and less to museums.
project CONTEXT (01) T
- projet commencé l’année dernière
- on a essayé de le faire marcher mais on a pas réussi
- du coup on a pris rdv avec les élèves de l’année dernière
- ils nous ont expliqué qu’on avait une très vieille version du projet et qu’il avait perdu
presques tout leur fichier (à part quelques modèle de vêtements)
- du coup on a vu avec notre encadrant Ronan qu’on allait repartir de 0
vu qu’on est reparti de zéro, on a modifié un peu le cahier des charges
for the project context, So this project was supposed to be a legacy project from last year
and we were supposed to take upon the work that have been done and improve it. But after
trying for a long time to run the project and a meeting with the previous team, we discovered
that the version we were given was missing key elements, which the team had also lost. Without those elements, we couldn’t work on the project, and after a meeting with our
supervisor, we decided to restart the project from the beginning.
STATE OF THE ART (02) C
OBJECTIVES (03) T
objectifs de choses à faire pour la soutenance de janvier
So our objective at the time were clear
As we started the project, we made a to do list. The first thing on the list was to explore the
project environment (hardware and software) and make the previous project work. The
environment wasn’t that hard, cause the precedent team left a trace of that, and so we easily
connected the Kinect to our computer, and started having decent output from the camera. We also discovered that the software going along with the Azure Kinect wasn’t available on
Ubuntu, and therefore we were forced to use Windows for all of the testing and coding. The
project itself missed key parts, so he wasn’t usable.
Then, once we agreed to restart from the beginning, we searched for ways to make a
functional project. For that, we started looking at various API that could permit us to
transpose our movement into Unity and synchronize them with 3D models.
But synchronizing those models
At the same time, we also took notice of which 3d models we got from the precedent team.
Once we settled down on an API, we now had to map the kinect skeleton joints to the 3d
models using blender. We also had to parameterize the API so it could run the way we
needed. Now, we’re currently working on mapping more 3d models, but we’re also trying to
add a background scene to our mirror and to improve the clothe’s physics.
FUNCTIONAL DESCRIPTION (04) Q
1. Tools and Software Stack
To bring this project to life, we utilized a specific set of tools tailored to our hardware
constraints:
● Azure Kinect & SDK: This served as our primary sensor for capturing data.
● kinect viewer
● Unity: The core engine used to develop the application.
● Nuitrack: We chose this for body tracking because the official Microsoft SDK
requires an NVIDIA graphics card, which was unavailable on our machines. Nuitrack
provided a reliable, cross-platform alternative.
● Blender: Essential for rigging and retargeting, allowing us to homogenize the various
character skeletons.
● + C#
2. Development Process
Our technical implementation followed a step-by-step progression:
● Initial Integration: We started by interfacing the Kinect with Unity to ensure a stable
video feed.
● Body Tracking Setup: Next, we integrated the Nuitrack API into Unity, successfully
mapping motion data onto a "stickman" model for testing.
● Avatar Implementation: We then downloaded a standard avatar from the Unity
Asset Store to master the skinning process.
● Legacy Integration: Once the workflow was established, we applied it to the custom
"Romain" skin provided by last year’s group.
3. Key Features and Technical Challenges
Following our supervisors' guidelines, we prioritized functionality over the user interface.
● Dynamic Customization: We developed a C# script that allows the user to switch
outfits in real-time without restarting the application.
● Cloth Physics (The Cape): To add realism, we integrated a cape. This required
adding specific vertices in Blender and using the Skinned Mesh Renderer in Unity. We then implemented a simplified physics model to simulate cloth behavior.
● 3D Environment: Finally, regarding the background decor, we are currently working
on adding one while addressing several optimization issues.
PROTOTYPE (05) T
WORKING STRATEGY (06) A
In order to share our work, we used Google Drive instead of other file sharing softwares or
sites such as gitlab, parce qu’il n’y a pas de code, et les fichiers 3D sont trop lourds pour
mettre dans gitlab il aurait fallu utiliser des versions de gitlab beacu coup plus compliqué à
gérer, et il fonctionne pas trop sur les fichiers de modélisations 3D, vu qu’on a très peu de
code, c’était pas très utile d’utiliser gitlab on a préféré utiliser une clé usb et google drive
Current problems :
- Mirror calibration not done (didn’t have time, prioritized having a working
bodytracking gig with skins)
- Background setting still not fixed (Unity)
- Helmets still need work (either Blender)
- Physics still need work, need to follow the body’s motions and add gravity for cape
physics
- Still have to go through Unity in order to run the project
