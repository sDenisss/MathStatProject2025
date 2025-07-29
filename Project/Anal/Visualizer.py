import pandas as pd

df = pd.read_csv("ExcelGraphs.csv")
import matplotlib.pyplot as plt

# Определим, кто победил
def get_winner(row):
    if row['Score1'] > row['Score2']:
        return 'Team1'
    elif row['Score2'] > row['Score1']:
        return 'Team2'
    else:
        return 'Draw'

df['Winner'] = df.apply(get_winner, axis=1)

# Фильтрация победителей и проигравших
winners = df[df['Winner'] != 'Draw'].copy()
losers = df[df['Winner'] != 'Draw'].copy()

# Разделение значений по победившей/проигравшей команде
def get_stat(row, stat1, stat2):
    return row[stat1] if row['Winner'] == 'Team1' else row[stat2]

for col in ['Possession', 'PassAccuracy', 'ShotsAccuracy', 'xG', 'Score']:
    winners[col] = winners.apply(lambda r: get_stat(r, f'{col}1', f'{col}2'), axis=1)
    losers[col] = losers.apply(lambda r: get_stat(r, f'{col}2', f'{col}1'), axis=1)

# Построение
stats = ['Possession', 'PassAccuracy', 'ShotsAccuracy', 'xG', 'Score']
win_means = winners[stats].mean()
lose_means = losers[stats].mean()

x = range(len(stats))
plt.figure(figsize=(10, 6))
plt.bar(x, win_means, width=0.4, label='Победители')
plt.bar([i + 0.4 for i in x], lose_means, width=0.4, label='Проигравшие')
plt.xticks([i + 0.2 for i in x], stats)
plt.ylabel("Среднее значение")
plt.title("Средние метрики: Победители vs Проигравшие")
plt.legend()
plt.tight_layout()
plt.savefig("avg_winner_loser.png")
plt.show()


import seaborn as sns

# Собираем все команды в одну таблицу
teams_data = pd.DataFrame({
    'Team': df['Team1'].tolist() + df['Team2'].tolist(),
    'xG': df['xG1'].tolist() + df['xG2'].tolist(),
    'Goals': df['Score1'].tolist() + df['Score2'].tolist()
})

plt.figure(figsize=(8, 6))
sns.scatterplot(data=teams_data, x='xG', y='Goals')
plt.title("Ожидаемые голы (xG) vs Реальные голы")
plt.xlabel("xG")
plt.ylabel("Голы")
plt.grid(True)
plt.savefig("xg_vs_goals.png")
plt.show()
