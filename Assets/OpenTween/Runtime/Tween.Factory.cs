using OpenTween.Jobs;

namespace OpenTween
{
    public struct Tween
    {
        public static Tween<T> Create<T>()
        {
            return Create(TweenOptions<T>.Default);
        }

        public static Tween<T> Create<T>(TweenOptions<T> options)
        {
            int id = TweenRegistry<T, TweenManagedReferences<T>>.New();

            ref TweenOptions<T> opt = ref TweenRegistry<T, TweenManagedReferences<T>>.GetOptionsByRef(id);
            opt.CopyFrom(options);

            ref TweenInternal<T> t = ref TweenRegistry<T, TweenManagedReferences<T>>.GetByRef(id);
            t.CurrentTime = 0;
            t.CurrentValue = options.Start;

            return new Tween<T>(id, t.Version);
        }
    }
}