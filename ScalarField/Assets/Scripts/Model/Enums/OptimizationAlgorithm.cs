namespace Model.Enums
{
    /// <summary>
    /// Enum containing all currently visualized strategies used in the scipy minimization function
    /// </summary>
    public enum OptimizationAlgorithm
    {
        SteepestDescent = 0,
        NelderMead = 1,
        Newton = 2,
        NewtonDiscrete = 3,
        NewtonTrusted = 4,
        Bfgs = 5
    }
}