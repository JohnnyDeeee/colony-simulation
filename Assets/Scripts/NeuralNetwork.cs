using System;
using System.Collections.Generic;
using System.Linq;

public class NeuralNetwork {
    private int inputLayerSize;
    private int hiddenLayerSize;
    private int outputLayerSize;
    private double epsilonInit = 0.12;

    private double[] inputNodes;
    private double[] hiddenNodes;
    private double[] outputNodes;

    private double[] inputWeights;
    private double[] hiddenWeights;

    public NeuralNetwork(int inputLayerSize, int hiddenLayerSize, int outputLayerSize, double[] inputWeights = null) {
        this.inputLayerSize = inputLayerSize;
        this.hiddenLayerSize = hiddenLayerSize;
        this.outputLayerSize = outputLayerSize;

        // Nodes
        this.inputNodes = new double[this.inputLayerSize +1]; // +1 for bias
        this.hiddenNodes = new double[this.hiddenLayerSize +1]; // +1 for bias
        this.outputNodes = new double[this.outputLayerSize];

        // Add bias nodes
        const int bias = 1;
        this.inputNodes[0] = bias;
        this.hiddenNodes[0] = bias;

        // Weights
        if(inputWeights == null) {
            this.inputWeights = new double[(this.inputLayerSize +1) * this.hiddenLayerSize]; // +1 for bias
            for(int i = 0; i < this.inputWeights.Length; i++) // Initialize weights randomly
                this.inputWeights[i] = ((UnityEngine.Random.value * 2) * epsilonInit) - epsilonInit;
        } else
            this.inputWeights = inputWeights;

        this.hiddenWeights = new double[(this.hiddenLayerSize +1)  * this.outputLayerSize]; // +1 for bias
        for(int i = 0; i < this.hiddenWeights.Length; i++) // Initialize weights randomly
            this.hiddenWeights[i] = ((UnityEngine.Random.value * 2) * epsilonInit) - epsilonInit;
    }

    public double[] FeedForward(double[] inputs) {
        if(inputs.Length != this.inputLayerSize)
            throw new Exception(string.Format("input size ({0}) does not match required size ({1})", inputs.Length, this.inputLayerSize));

        // Normalize input
        double[] normalizedInput = this.MinMaxNormalization(inputs);

        for(int i = 1; i < this.inputNodes.Length; i++) // Start at 1 to skip bias node
            this.inputNodes[i] = normalizedInput[i-1];

        // Activate hidden layer
        for(int a = 1; a < this.hiddenNodes.Length; a++) { // Start at 1 to skip bias node
            double sum = 0;
            for(int w = 0; w < this.inputLayerSize; w++) {
                int index = (this.inputLayerSize * a) - this.inputLayerSize + w;
                sum += this.inputWeights[index] * this.inputNodes[w];
            }
            this.hiddenNodes[a] = this.Sigmoid(sum);
        }

        // Activate output layer
        for(int a = 0; a < this.outputNodes.Length; a++) {
            double sum = 0;
            for(int w = 0; w < this.hiddenLayerSize; w++) {
                int index = (this.hiddenLayerSize * a) + w;
                sum += this.hiddenWeights[index] * this.hiddenNodes[w];
            }
            this.outputNodes[a] = this.Sigmoid(sum);
        }

        return this.outputNodes;
    }

    private double Sigmoid(double x) {
        return 1.0/(Math.Exp(-x) + 1.0);
    }

    private double[] MinMaxNormalization(double[] input) {
        double[] normalizedInput = new double[input.Length];
        
        for(int i = 0; i < input.Length; i++) {
            double normalizedValue = (input[i] - input.Min()) / (input.Max() - input.Min());
            normalizedValue = Double.IsNaN(normalizedValue) ? 0 : normalizedValue; // Convert NaN into 0

            normalizedInput[i] = normalizedValue;
        }

        return normalizedInput;
    }

    public int GetInputLayerSize() { return this.inputLayerSize; }
    public int GetHiddenLayerSize() { return this.hiddenLayerSize; }
    public int GetOutputLayerSize() { return this.outputLayerSize; }
    public double[] GetInputWeights() { return this.inputWeights; }
}