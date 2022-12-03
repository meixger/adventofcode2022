f = open('input.txt', 'r', encoding='utf-8-sig')
rucksacks = [line.rstrip() for line in f.readlines()]

itemTypesInt = list(range(97, 123)) + list(range(65, 91))
itemTypes = list(map(chr, itemTypesInt))

solutionA = 0
for rucksack in rucksacks:
    length = len(rucksack)
    compartement1 = rucksack[0:int(length/2)]
    compartement2 = rucksack[int(length/2):length]
    for it in itemTypes:
        if compartement1.find(it) > -1 and compartement2.find(it) > -1:
            solutionA += itemTypes.index(it) + 1

print("SolutionA: {}".format(solutionA))


solutionB = 0
groups = [rucksacks[i:i + 3] for i in range(0, len(rucksacks), 3)]
for g in groups:
    for it in itemTypes:
        if all(x.find(it) > -1 for x in g):
            solutionB += itemTypes.index(it) + 1

print("SolutionB: {}".format(solutionB))
