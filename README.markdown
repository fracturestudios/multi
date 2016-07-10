
## Networked Multiplayer on Unity

A work in progress

### Initial Testbed Design

For the initial networking testbed, our goal is to simulate real networking in terms of GameObjects and components, but without actually having to deal with networks.
These components will be designed as if they were really sending packets over the network, but in reality they'll be directly writing packets into each others' buffers.
This will be mediated through a 'fake network endpoint' class that also handles simulating network unreliability / jittered latency.

Core network management will be simulated using NetworkClient and NetworkServer game objects. 
For the testbed, these will be concrete classes, but long-term these ought to be factored down to abstract bases and put in a client library you can extend later.

NetworkServer will be responsible for routinely waking up and sending world diffs to its clients.

NetworkClient will be responsible for collecting player inputs, and routinely waking up to send to the client.

Both NetworkServer and NetworkClient belong in standalone / scene-level entities.

By default, NetworkServer and NetworkClient replicate using reflection on plain game objects.
NetworkClient is responsible for interpolation:

* Numeric values are linearly interpolated
* All other values are 'updated' at the start of the keyframe that first modifies them

NetworkClient additionally predicts by extrapolating numeric values and blending predicted state with server-authoritative state.

To override any of this behavior, programmers make new components derived from the following objects:

* NetworkReplication - Changes how an object or component is serialized
* NetworkInterpolation - Changes how an object or component is serialized
* NetworkPrediction - Changes how an object or component is predicted

You can attach multiple derivations of these components to the same game object.
They form a filtering chain, where each component has an altitude and can preprocess, postprocess, or skip the components below (i.e. with lesser altitude).
Each item in the filtering chain can register itself for any component on the game object.
If an item registers on a component it chooses not to modify, it can pass and let the lower links of the chain do their processing.
The bottom link in the filtering chain are the default implementations for replication, interpolation and prediction mentioned above.

