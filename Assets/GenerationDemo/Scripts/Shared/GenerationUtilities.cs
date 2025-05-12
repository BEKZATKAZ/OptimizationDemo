namespace GenerationDemo.Shared
{
    public static class GenerationUtilities
    {
        public static int GetVertexCount(int resolution) => resolution * resolution;
        public static int GetIndexCount(int resolution) => (resolution - 1) * (resolution - 1) * 6;
    }
}
