# Stickman-Walker

Using Unity's ML Agents to train stickmen to walk using curiosity. Featured in the YouTube video here: https://youtu.be/ymzLGRJSkvg

Hopefully this repo is usable by other people.

Version information:
Unity version 2020.2.2f1.
ML Agents release 14 (Unity package v1.8.1, Python package v0.24.1). You may have to install this manually.
ML Agents can be found here: https://github.com/Unity-Technologies/ml-agents .
Release 14 can be found here: https://github.com/Unity-Technologies/ml-agents/tree/com.unity.ml-agents_1.0.7 .

How the stickmen were trained:

The No Curiosity Agent was trained using an intrinsic reward value set to 0. It learned to gallop.

The Curiosity Agent was trained using an intrinsic reward value set to 0.03. It learned to walk.

The From Curiosity Agent was initialized from the Curiosity Agent with a new curiosity of 0.0001. It learned to walk because it took its knowledge from the Curiosity Agent.
This agent is likely better simply because it had more effective training steps, but effectively removing the curiosity after it learned to walk may also have had an effect.
