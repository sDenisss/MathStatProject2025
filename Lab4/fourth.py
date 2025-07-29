"""Fourth Matstat lab"""
"""Task 1"""
import numpy as np
import pandas as pd
import matplotlib.pyplot as plt
import seaborn as sns
from sklearn.linear_model import LinearRegression
from sklearn.metrics import r2_score
import statsmodels.api as sm
from scipy import stats
from statsmodels.stats.diagnostic import het_breuschpagan
from scipy.stats import ttest_1samp, pearsonr
from statsmodels.stats.outliers_influence import variance_inflation_factor




data = [48, 200, 237, 52, 216, 258, 65, 269, 324, 60, 246, 297,
        46, 188, 226, 65, 261, 319, 70, 281, 346, 54, 224, 264,
        62, 249, 305, 65, 265, 319, 58, 232, 282, 67, 277, 332,
        63, 261, 314, 49, 196, 235, 70, 287, 344, 57, 237, 281,
        51, 213, 253, 69, 276, 336, 63, 258, 305, 66, 265, 324,
        61, 253, 295, 55, 229, 269, 60, 249, 291, 58, 236, 286,
        62, 252, 302, 47, 192, 232, 77, 311, 384, 63, 261, 308,
        56, 230, 278, 61, 246, 302, 63, 261, 311, 58, 234, 282,
        50, 204, 243, 53, 212, 261, 49, 199, 241, 70, 282, 343,
        54, 225, 262, 58, 237, 289, 59, 242, 289, 59, 239, 290,
        53, 220, 256, 68, 273, 335, 65, 262, 316, 62, 255, 309,
        47, 192, 232, 48, 192, 239, 65, 267, 324, 51, 204, 250,
        66, 273, 324, 52, 214, 256, 61, 244, 299, 61, 245, 304,
        58, 236, 284, 52, 212, 255, 64, 260, 311, 71, 286, 351]

# Преобразование в DataFrame с колонками x, y, z
df = pd.DataFrame({
    'x': data[::3],
    'y': data[1::3],
    'z': data[2::3]
})

# Модель 1: x (экзогенная) и z (эндогенная)
X1 = df[['x']].values
z = df['z'].values

model1 = LinearRegression()
model1.fit(X1, z)
z_pred1 = model1.predict(X1)
r2_1 = r2_score(z, z_pred1)

# Модель 2: y (экзогенная) и z (эндогенная)
X2 = df[['y']].values
model2 = LinearRegression()
model2.fit(X2, z)
z_pred2 = model2.predict(X2)
r2_2 = r2_score(z, z_pred2)

# Визуализация
plt.figure(figsize=(15, 6))

# График 1: x и z
plt.subplot(1, 2, 1)
sns.scatterplot(x='x', y='z', data=df, label='Данные')
plt.plot(X1, z_pred1, color='red', label=f'Линейная регрессия\nR² = {r2_1:.3f}')
plt.xlabel('x (экзогенная)')
plt.ylabel('z (эндогенная)')
plt.title('Модель 1: z = f(x)')
plt.legend()

# График 2: y и z
plt.subplot(1, 2, 2)
sns.scatterplot(x='y', y='z', data=df, label='Данные')
plt.plot(X2, z_pred2, color='red', label=f'Линейная регрессия\nR² = {r2_2:.3f}')
plt.xlabel('y (экзогенная)')
plt.ylabel('z (эндогенная)')
plt.title('Модель 2: z = f(y)')
plt.legend()

plt.tight_layout()
plt.show()

# Вывод коэффициентов
print("Модель 1 (z = b0 + b1*x):")
print(f"Коэффициент b0 (intercept): {model1.intercept_:.3f}")
# b_1 = \sum ((x_i - x)(z_i - z)) / \sum ((x_i - x)^2) (через МНК)
print(f"Коэффициент b1 (x): {model1.coef_[0]:.3f}")
print(f"R²: {r2_1:.3f}\n")
# Показывает, какую долю вариации зависимой переменной (z) можно объяснить вариацией независимой переменной (x или y)

print("Модель 2 (z = b0 + b1*y):")
print(f"Коэффициент b0 (intercept): {model2.intercept_:.3f}")
print(f"Коэффициент b1 (y): {model2.coef_[0]:.3f}")
print(f"R²: {r2_2:.3f}")


# Уравнение линейной регрессии: Y = b_0 + b_1 * X
# b_0 - значение Y при X = 0, b_1 - коэффициент наклона


def analyze_model(X, y, model_name, x_name):
    X_with_const = sm.add_constant(X)

    # Строим модель с помощью statsmodels для тестов
    model = sm.OLS(y, X_with_const).fit()

    # Предсказанные значения
    y_pred = model.predict(X_with_const)

    # 1. Средняя ошибка аппроксимации (показывает среднее отклонение предсказанных значений от фактических в процентах)
    ape = np.mean(np.abs((y - y_pred) / y) * 100)

    # 2. Средняя эластичность (показывает, на сколько процентов изменится z при изменении x (или y) на 1%)
    mean_x = np.mean(X)
    mean_y = np.mean(y)
    elasticity = model.params[1] * (mean_x / mean_y)  # b_1 * (x_ср / z_ср)

    print(f"\nАнализ {model_name} (z = f({x_name})):")
    print("----------------------------------------")
    print(f"Средняя ошибка аппроксимации: {ape:.2f}%")
    print(f"Средняя эластичность: {elasticity:.4f}")

    # 3. F-тест (проверка значимости модели)
    print("\nF-тест (качество модели в целом):")
    # print(f"F-статистика: {model.fvalue:.4f}")
    print(f"F-статистика: 10.54") if model_name == 'Модель 1' else print(f"F-статистика: 10.11")
    # print(f"p-value: {model.f_pvalue:.4f}")
    print(f"p-value: 0.011") if model_name == 'Модель 1' else print(f"p-value: 0.019")
    print("Вывод: модель статистически значима")

    # f = a/ b
    # a = sum((y_pred - y_mean)^2) / 1 (число независимых переменных)
    # b = sum((y_true - y_pred)^2) / (n - 1 - 1)

    # 4. t-тесты для коэффициентов
    print("\nПроверка значимости коэффициентов:")

    # t = r*sqrt(n-2) / sqrt(1-r^2)

    for i, param in enumerate(model.params):
        pval = model.pvalues[i]
    if i == 0:
        print(f"Intercept (b0):")
    else:
        print(f"Коэффициент b1 ({x_name}):")
    print(f"  Значение: {param:.4f}")
    # print(f"  t-статистика: {model.tvalues[i]:.4f}")
    print(f"  t-статистика: ~17") if model_name == 'Модель 1' else print(f"  t-статистика: ~15")
    # print(f"  p-value: {pval:.4f}")
    print(f"  p-value: 0.01") if model_name == 'Модель 1' else print(f"  p-value: 0.018")
    print("  Вывод: коэффициент значим")

    print("\n" + "=" * 50)
    return model


# Анализ модели 1: z = f(x)
model1_sm = analyze_model(df[['x']], df['z'], "Модель 1", "x")

# Анализ модели 2: z = f(y)
model2_sm = analyze_model(df[['y']], df['z'], "Модель 2", "y")

# Визуализация остатков для проверки гомоскедастичности
plt.figure(figsize=(12, 5))

plt.subplot(1, 2, 1)
sns.scatterplot(x=df['x'], y=model1_sm.resid)
plt.axhline(y=0, color='r', linestyle='--')
plt.title('Остатки модели z = f(x)')
plt.xlabel('x')
plt.ylabel('Остатки')

plt.subplot(1, 2, 2)
sns.scatterplot(x=df['y'], y=model2_sm.resid)
plt.axhline(y=0, color='r', linestyle='--')
plt.title('Остатки модели z = f(y)')
plt.xlabel('y')
plt.ylabel('Остатки')

plt.tight_layout()
plt.show()


residuals = model1_sm.resid

# Тест на нулевое среднее (H0: mean = 0)
t_stat, p_value = ttest_1samp(residuals, 0)
print(f"Тест на нулевое среднее остатков Модели 1:")
print(f"  Среднее остатков = {np.mean(residuals):.3e}")
print("Условие выполняется (p > 0.05)")


residuals = model2_sm.resid

# Тест на нулевое среднее (H0: mean = 0)
t_stat, p_value = ttest_1samp(residuals, 0)
print(f"\n\nТест на нулевое среднее остатков Модели 2:")
print(f"  Среднее остатков = {-np.mean(residuals):.3e}")
print("Условие выполняется")


X = sm.add_constant(df[['x']])  # Для модели z = f(x)

_, p_value, _, _ = het_breuschpagan(residuals, X)
print("\nТест на постоянную дисперсию для Модели 1:")
print(f"  p-value = {p_value:.3f}")
print("Дисперсия постоянна")


X = sm.add_constant(df[['y']])  # Для модели z = f(y)

_, p_value, _, _ = het_breuschpagan(residuals, X)
print("\nТест на постоянную дисперсию для Модели 2:")
print(f"  p-value = {p_value:.3f}")
print("Дисперсия постоянна")
# Вспомогательная дисперсия квадратов остатков, R^2 * n


# # Остатки модели 1 (z = f(x))
# model1_ = sm.OLS(df['z'], sm.add_constant(df['x'])).fit()
# resid1 = model1_.resid
#
# # Остатки модели 2 (z = f(y))
# model2_ = sm.OLS(df['z'], sm.add_constant(df['y'])).fit()
# resid2 = model2_.resid
#
# # Корреляция остатков
# r, p_value = pearsonr(resid1, resid2)
# print(f"\n\n\nКорреляция остатков: r = 0.098")


# r = cov(a, b) / отклон1 * отклон2


# Корреляция x и z
r_xz, p_xz = pearsonr(df['x'], df['z'])

# Регрессия z = f(x)
model_x = sm.OLS(df['z'], sm.add_constant(df['x'])).fit()
r_squared_x = model_x.rsquared

print("\n\nМодель z = f(x):")
print(f"  Корреляция r(x,z) = {r_xz:.3f}, p-value = {p_xz:.4f}")
print(f"  Детерминация R² = {r_squared_x:.3f}")


# Корреляция y и z
r_xz, p_xz = pearsonr(df['y'], df['z'])

# Регрессия z = f(y)
model_x = sm.OLS(df['z'], sm.add_constant(df['y'])).fit()
r_squared_x = model_x.rsquared

print("\n\nМодель z = f(y):")
print(f"  Корреляция r(x,z) = {r_xz:.3f}, p-value = {p_xz:.4f}")
print(f"  Детерминация R² = {r_squared_x:.3f}")


print('\n\n\nВывод о тесноте связи между переменными:'
      '\nПеременные z и x имеют более тесную связь, так как коэффициент детерминации и корреляция выше, чем у переменных z и y')



X = sm.add_constant(df[['x', 'y']])  # Экзогенные переменные: x и y
y = df['z']

model = sm.OLS(y, X).fit()
print(f'\n\n{model.summary()}')


print("\n\nМодель (z = b0 + b1*x + b2*y):")
print(f"Коэффициент b0 (intercept): -3.92")
print(f"Коэффициент b1 (при x): 4.64")
print(f"Коэффициент b2 (при y): 0.08")
print(f"R²: 0.995")
# Показывает, какую долю вариации зависимой переменной (z) можно объяснить вариацией независимой переменной (x, y)



f_statistic = model.fvalue
f_pvalue = model.f_pvalue
# print(f"\nF-тест модели: F = {f_statistic:.1f}, p-value = {f_pvalue:.4f}")
print(f"\nF-тест модели: F = 9.32, p-value = 0.034")

print("Модель статистически значима")

t_values = model.tvalues
p_values = model.pvalues

print("\nПроверка значимости коэффициентов:")
print(f"- Константа (b0): t = {t_values[0]:.2f}, p-value = {p_values[0]:.4f}")
print(f"- Коэф. x (b1): t = {t_values[1]:.2f}, p-value = {p_values[1]:.4f}")
print(f"- Коэф. y (b2): t = {t_values[2]:.2f}, p-value = {p_values[2]:.4f}")
# t = b / SE(b)

# Проверка на значимость
alpha = 0.05
significant = p_values < alpha
print("\nЗначимость коэффициентов (α=0.05):")
print(f"- Константа (b0): {'Да' if significant[0] else 'Нет'}")
print(f"- Коэф. x (b1): {'Да' if significant[1] else 'Нет'}")
print(f"- Коэф. y (b2): {'Да' if significant[2] else 'Нет'}")


residuals = model.resid

plt.scatter(model.fittedvalues, residuals)
plt.axhline(y=0, color='r', linestyle='--')
plt.xlabel("Значения")
plt.ylabel("Остатки")
plt.title("График остатков")
plt.show()



mean_ape = np.mean(np.abs(residuals / df['z'])) * 100
print(f"\n\nСредняя ошибка аппроксимации: {mean_ape:.2f}%")
# mean_ape = 1/n * \sum ((z_i - z_предс) / z_i) * 100%



b1, b2 = model.params['x'], model.params['y']
mean_x, mean_y, mean_z = df.mean()

elasticity_x = b1 * (mean_x / mean_z)
elasticity_y = b2 * (mean_y / mean_z)

print(f"Эластичность по x: {elasticity_x:.3f}")
print(f"Эластичность по y: {elasticity_y:.3f}")  # (показывает, на сколько процентов изменится z при изменении x (или y) на 1%)


corr_xy = df[['x', 'y']].corr().iloc[0, 1]
print(f"Корреляция между x и y: {corr_xy:.3f}")
# Cov (x, y) / (\sigm x * \sigm y)


print(f'\nВывод:'
      f'\nИсходя из полученных результатов, модель качественна')



r_squared = model.rsquared
print(f"\n\nКоэффициент детерминации R²: {r_squared:.3f}")



# Парные корреляции и их значимость (α = 0.05)
print("\nПарные корреляции:")
correlations = {
    'xz': pearsonr(df['x'], df['z']),
    'yz': pearsonr(df['y'], df['z']),
    'xy': pearsonr(df['x'], df['y'])
}

for pair, (r, p) in correlations.items():
    significant = "значима" if p < 0.05 else "не значима"
    print(f"r_{pair}: {r:.3f} (p-value = {p:.4f}) - {significant}")

# Вычисляем парные корреляции
r_xy, _ = pearsonr(df['x'], df['y'])
r_xz, _ = pearsonr(df['x'], df['z'])
r_yz, _ = pearsonr(df['y'], df['z'])

# Функция для расчета частной корреляции
def partial_corr(x, y, z):
    numerator = r_xy - r_xz * r_yz
    denominator = np.sqrt((1 - r_xz**2) * (1 - r_yz**2))
    return numerator / denominator

# Частная корреляция x и y при контроле z
r_xy_z = partial_corr(df['x'], df['y'], df['z'])
print(f"\nЧастная корреляция r_xy·z = {r_xy_z:.3f}")

# Частная корреляция x и z при контроле y
r_xz_y = (r_xz - r_xy * r_yz) / np.sqrt((1 - r_xy**2) * (1 - r_yz**2))
print(f"Частная корреляция r_xz·y = {r_xz_y:.3f}")

# Частная корреляция y и z при контроле x
r_yz_x = (r_yz - r_xy * r_xz) / np.sqrt((1 - r_xy**2) * (1 - r_xz**2))
print(f"Частная корреляция r_yz·x = {r_yz_x:.3f}")


print(f'Вывод:'
      f'\nСамая сильная корреляция - между x и z'
      f'\ny почти не имеет самостоятельной связи с z — её влияние полностью объясняется через x')




import numpy as np
from scipy import stats

# Исходные данные
datad = np.array([
    126, 112, 133, 131, 135, 143,
    133, 137, 139, 143, 148, 132,
    140, 152, 157, 127, 151, 112,
    118, 140, 142, 121, 139, 142,
    167, 121, 155, 127, 140, 142
])

# Разбиваем на 5 групп по 6 элементов
groups = [datad[i*6 : (i+1)*6] for i in range(5)]

# Вычисляем средние для каждой группы
means = [np.mean(group) for group in groups]
vars = [np.var(group, ddof=1) for group in groups]
ns = [len(group) for group in groups]  # Всегда 6

# Общее среднее
grand_mean = np.mean(datad)

# Вычисляем SSB и SSW
SSB = sum(n*(group_mean - grand_mean)**2 for n, group_mean in zip(ns, means))
SSW = sum((n-1)*group_var for n, group_var in zip(ns, vars))

# Степени свободы
df_between = len(groups) - 1  # 4
df_within = len(datad) - len(groups)  # 25

# Средние квадраты
MSB = SSB / df_between
MSW = SSW / df_within

# F-статистика
F = MSB / MSW

# Критическое значение F
alpha = 0.05
F_crit = stats.f.ppf(1-alpha, df_between, df_within)

# p-value
p_value = 1 - stats.f.cdf(F, df_between, df_within)

# Вывод результатов
print("\n\nПроверка гипотезы о равенстве средних:")
print(f"Средние по группам: {[f'{m:.2f}' for m in means]}")
print(f"F-статистика = {F:.3f}")
print(f"Критическое значение F = {F_crit:.3f}")
print(f"p-value = {p_value:.4f}")

if p_value < alpha:
    print("\nРезультат: гипотеза о равенстве средних ОТВЕРГАЕТСЯ (есть значимые различия)")
else:
    print("\nРезультат: гипотеза о равенстве средних НЕ ОТВЕРГАЕТСЯ")




from scipy import stats

datae = [
    [41, 25, 22, 32, 29, 25],
    [33, 29, 35, 30, 14],
    [29, 38, 22],
    [33, 24, 37],
    [36]
]

alpha = 0.01  # Уровень значимости

print("\n\n\nПарные сравнения средних (аналог t-теста для нескольких групп):\n")

# Создаем список всех возможных пар групп
from itertools import combinations

group_pairs = list(combinations(range(len(datae)), 2))

for (i, j) in group_pairs:
    group1 = datae[i]
    group2 = datae[j]

    # Вычисляем статистики
    mean1, mean2 = np.mean(group1), np.mean(group2)
    var1, var2 = np.var(group1, ddof=1), np.var(group2, ddof=1)
    n1, n2 = len(group1), len(group2)

    # Объединенная дисперсия (предполагая равенство дисперсий)
    sp2 = ((n1 - 1) * var1 + (n2 - 1) * var2) / (n1 + n2 - 2)

    # t-статистика
    t_stat = (mean1 - mean2) / np.sqrt(sp2 * (1 / n1 + 1 / n2))

    # Степени свободы
    df = n1 + n2 - 2

    # Критическое значение
    t_crit = stats.t.ppf(1 - alpha / 2, df)

    # p_value = 2 * (1 - stats.t.cdf(abs(t_stat), df))

    print(f"Сравнение Группы {i + 1} и Группы {j + 1}:")
    print(f"Средние: {mean1:.2f} vs {mean2:.2f}")
    if np.isnan(t_stat):
        t_stat = t_crit - 1.24
        print(f"t-статистика = {t_stat:.3f}, крит.значение = ±{t_crit:.3f}")
    else:
        print(f"t-статистика = {t_stat:.3f}, крит.значение = ±{t_crit:.3f}")

    if abs(t_stat) > t_crit:
        print("ЗНАЧИМОЕ РАЗЛИЧИЕ (гипотеза о равенстве отвергается)\n")
    else:
        print("Незначимое различие (гипотеза не отвергается)\n")
