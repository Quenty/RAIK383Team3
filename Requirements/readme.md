# Release candidates
Release candidates emerge in develop when a good, stable set of features have been merged in, and their integration tested. Each release candidate will be pushed to the master branch where I can review it (at the two checkpoints during implementation later in the term). That way, while I review your master versions, you can continue working on your develop versions. 

# Requirements folder
In the team repo folder structure, at the same level as the project README, make a folder called 'Requirements' where you will write up your requirements for this assignment. Each requirement should be a scenario with the structure discussed in class (and in the book, Figure 4.10 in the 10th Edition, the iLearn Uploading Photos scenario; note that the iLearn system example is not in the 9th Edition of the book). Each scenario should be in a separate file under Requirements/. Please name each file suggestively in regards to the code feature it describes (for the given example that would be uploadingPhotos).

# Scenarios
A structured form of user story
- Scenarios should include
    - A description of the starting situation;
    - A description of the normal flow of events;
    - A description of what can go wrong;
    - Information about other concurrent activities;
    - A description of the state when the scenario finishes.

# Notes

- start working from the initial project story I gave in my course project description and refining that into your set of requirements by picking up on the usage - suggested in that story, and filling in the missing details about what different user roles are needed, and what a typical user in eac role needs from your system a customer is not available on this project to help clarify the requirements, but the domain chosen, that of transportation, should be familiar enough so you can use your knowledge about it to come up with realistic requirements
- feel free to consult with the assigned TA or with me if you need further clarifications make sure to include a requirement about the optimality of your solution, as mentioned in the initial project story; for that, you need to come up with a cost model; you are free to make assumptions about the cost model, as long as they don't make the problem too easy (trivial), or make it too hard to be implementable in the project timeframe; your optimality requirement should not describe your solution, but describe the cost model and what you'll be optimizing, in layman terms
- as a rough estimate, each requirement should describe a feature for which a team member could make a feature branch in git and manage to implement it during a sprint; with about 8 weeks of implementation-only time on the project, you'll have two major sprints of 3 weeks each, with one week estimated for merging and integration testing before delivery at the end of each sprint (checkpoint for my review); that means you should have requirements for about 8 features for a team of 4 (10 for a team of 5) for the two sprints
- since your requirements will be in git, you can manage them in there during the project should they change, but they should be ready for review by the deadline of this assignment (February 10th) and that's the version you'll be graded on; I will basically validate your requirements at that point for completeness, consistency, etc.; if you will later make your feature branches related to your requirements (by naming them) then your requirements will also be easily traceable; when you will write your tests, you can also relate those to the features and the requirements, that would address the testability of your requirements
