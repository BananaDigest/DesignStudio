using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;

/// <summary>
/// Атрибут для автоматичної генерації даних AutoFixture у теоріях XUnit,
/// який:
/// 1) Налаштовує AutoFixture з підтримкою NSubstitute (ConfigureMembers=true).
/// 2) Видаляє стандартну поведінку ThrowingRecursionBehavior (яка кидає виняток при рекурсії).
/// 3) Додає OmitOnRecursionBehavior, щоб безпечно ігнорувати циклічні посилання в об'єктах.
/// Таким чином тестові дані генеруються без помилок через зацикленість моделей.
/// </summary>
public class SafeAutoDataAttribute : AutoDataAttribute
{
    public SafeAutoDataAttribute()
        : base(() =>
        {
            var fixture = new Fixture()
                .Customize(new AutoNSubstituteCustomization { ConfigureMembers = true });

            // Remove the default behavior that throws on recursion
            var toRemove = fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList();
            foreach (var b in toRemove)
                fixture.Behaviors.Remove(b);

            // Add behavior to omit recursion
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            return fixture;
        })
    {
    }
}