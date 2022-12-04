import { promises as fs } from "fs";

const readInputLines = async () => {
  var file = await fs.readFile('input.txt', 'utf-8');
  var lines = file.split(/\r?\n/).filter(l => l)
  return lines;
};

const transformLinesIntoPairs = (lines) => lines.map(line => line.split(',')).map(pair => pair.map(p => p.split('-').map(s => Number(s))))

async function SolutionA() {
  const lines = await readInputLines()
  const pairs = transformLinesIntoPairs(lines);

  const contains = (first, second) => first[0] >= second[0] && first[1] <= second[1];

  let count = 0;
  pairs.forEach(p => {
    const [left, right] = [p[0], p[1]];
    if (contains(left, right) || contains(right, left))
      count += 1;
  });
  console.log(`SolutionA: ${count}`);
}

async function SolutionB() {
  const lines = await readInputLines()
  const pairs = transformLinesIntoPairs(lines);

  const overlaps = (first, second) => first[1] >= second[0] && second[1] >= first[0];

  let count = 0;
  pairs.forEach(p => {
    const [left, right] = [p[0], p[1]];
    if (overlaps(left, right))
      count += 1;
  });
  console.log(`SolutionB: ${count}`)
}

export { SolutionA, SolutionB }