public class GeneticAlgorithm {
    // TODO: Selection
    //  1. Normalize each fitness (normFitness = fitness / sum(allFitnesses)), so that the sum of all fitnesses is 1
    //  2. Sort population desc fitness
    //  3. Accumulate normalized fitnesses (accFitness = normFitness + allPreviousNormFitnesses)
    //      ex: accFitness1 would be (normFitness1), accFitness2 would be (normFitness2 + normFitness1)
    //          accFitness3 would be (normFitness3 + normFitness2 + normFitness1) ...
    //  4. Choose random number R between 0 and 1
    //  5. Winner is the last one whose accFitness >= R
    // https://en.wikipedia.org/wiki/Selection_(genetic_algorithm)

    // TODO: Crossover
    //  Single point crossover
    //  1. Randomly pick a spot in the parents' chromosomes
    //  2. Bits to the right of the point are swapped
    // https://en.wikipedia.org/wiki/Crossover_(genetic_algorithm)

    // TODO: Mutation
    //  Flip Bit
    //  1. Randomly pick a spot in the child chromosomes
    //  2. Flip the bit on that spot (0 => 1, 1 => 0)
    // https://en.wikipedia.org/wiki/Mutation_(genetic_algorithm)
}