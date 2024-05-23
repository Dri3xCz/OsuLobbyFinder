namespace OsuMultiplayerLobbyFinder
{
    public abstract class Either<E, T>
    {
        public abstract void Fold(Action<E> leftMethod, Action<T> rightMethod);
    }

    public class Left<E, T> : Either<E, T>
    {
        E value;

        public Left(E value)
        {
            this.value = value;
        }

        public override void Fold(Action<E> leftMethod, Action<T> rightMethod)
        {
            leftMethod(value);
        }
    }

    public class Right<E, T> : Either<E, T>
    {
        T value;

        public Right(T value)
        {
            this.value = value;
        }

        public override void Fold(Action<E> leftMethod, Action<T> rightMethod)
        {
            rightMethod(value);
        }
    }
}