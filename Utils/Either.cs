namespace OsuMultiplayerLobbyFinder.Utils
{
    public abstract class Either<TLeft, TRight>
    {
        public abstract void Fold(Action<TLeft> leftMethod, Action<TRight> rightMethod);

        public abstract void Map(Action<TRight> rightMethod);
    }

    public class Left<TLeft, TRight> : Either<TLeft, TRight>
    {
        private readonly TLeft _value;

        public Left(TLeft value)
        {
            this._value = value;
        }

        public override void Fold(Action<TLeft> leftMethod, Action<TRight> rightMethod)
        {
            leftMethod(_value);
        }

        public override void Map(Action<TRight> rightMethod) {}
    }

    public class Right<TLeft, TRight> : Either<TLeft, TRight>
    {
        private readonly TRight _value;

        public Right(TRight value)
        {
            this._value = value;
        }

        public override void Fold(Action<TLeft> leftMethod, Action<TRight> rightMethod)
        {
            rightMethod(_value);
        }

        public override void Map(Action<TRight> rightMethod)
        {
            rightMethod(_value);
        }
    }
}