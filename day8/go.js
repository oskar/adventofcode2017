var fs = require("fs");

const getRegisterValue = register => registers[register] || 0;

const writeRegister = (register, instruction, value) => {
  const registerValue = getRegisterValue(register);
  switch (instruction) {
    case "inc":
      registers[register] = registerValue + value;
      break;
    case "dec":
      registers[register] = registerValue - value;
      break;
    default:
      throw Error("Unknown instruction:", instruction);
  }

  if (registers[register] > maxValue) {
    maxValue = registers[register];
  }
};

const evalCondition = (register, operator, value) => {
  const registerValue = getRegisterValue(register);
  switch (operator) {
    case ">":
      return registerValue > value;
    case "<":
      return registerValue < value;
    case ">=":
      return registerValue >= value;
    case "<=":
      return registerValue <= value;
    case "==":
      return registerValue === value;
    case "!=":
      return registerValue !== value;
    default:
      throw Error("Unknown operator:", operator);
  }
};

var maxValue = 0;
var registers = {};

fs.readFileSync("input.txt")
  .toString()
  .trimRight()
  .split("\n")
  .map(l => {
    const [
      register,
      instruction,
      value,
      _,
      condRegister,
      condOperator,
      condValue
    ] = l.split(" ");
    return {
      register,
      instruction,
      value: parseInt(value),
      condRegister,
      condOperator,
      condValue: parseInt(condValue)
    };
  })
  .forEach(l => {
    if (evalCondition(l.condRegister, l.condOperator, l.condValue)) {
      writeRegister(l.register, l.instruction, l.value);
    }
  });

console.log(registers);
console.log("Largest value during execution: ", maxValue);
console.log("Current largest value:", Math.max(...Object.values(registers)));
