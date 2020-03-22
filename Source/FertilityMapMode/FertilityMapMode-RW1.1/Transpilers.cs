using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using System.Reflection.Emit;
using System.Reflection;

namespace FertilityMapMode
{
	public static class Transpilers
	{
		/// <summary>
		/// Add a call to a method after a target method.
		/// </summary>
		/// <param name="instructions">The code instructions to transpile.</param>
		/// <param name="target">The target method after which to add the method.</param>
		/// <param name="value">The method to add after the target method.</param>
		/// <returns>The transpiled code instructions.</returns>
		/// <exception cref="ArgumentNullException">Thrown when instructions, target or value is null.</exception>
		public static IEnumerable<CodeInstruction> AddCallAfter(this IEnumerable<CodeInstruction> instructions, MethodInfo target, MethodInfo value)
		{
			if (instructions == null)
			{
				throw new ArgumentNullException(nameof(instructions));
			}
			if (target == null)
			{
				throw new ArgumentNullException(nameof(target));
			}
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			foreach (var instruction in instructions)
			{
				yield return instruction;

				if (instruction.Calls(target))
				{
					yield return new CodeInstruction(opcode: OpCodes.Call, operand: value);
				}
			}
		}

		/// <summary>
		/// Replaces all instances of the target field being loaded to the value field being loaded.
		/// </summary>
		/// <param name="instructions">The code instructions to transpile.</param>
		/// <param name="target">The target field to replace.</param>
		/// <param name="value">The field with which to replace the target field.</param>
		/// <returns>The transpiled code instructions.</returns>
		/// <exception cref="ArgumentNullException">Thrown when instructions, target or value is null.</exception>
		public static IEnumerable<CodeInstruction> ReplaceFieldLoad(this IEnumerable<CodeInstruction> instructions, FieldInfo target, FieldInfo value)
		{
			if (instructions == null)
			{
				throw new ArgumentNullException(nameof(instructions));
			}
			if (target == null)
			{
				throw new ArgumentNullException(nameof(target));
			}
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			foreach (var instruction in instructions)
			{
				if (instruction.LoadsField(target))
				{
					instruction.operand = value;
				}
				yield return instruction;
			}
		}

		/// <summary>
		/// Skips any calls to the given method.
		/// </summary>
		/// <param name="instructions">The code instructions to transpile.</param>
		/// <param name="method">The method to skip.</param>
		/// <returns>The transpiled instructions.</returns>
		/// <exception cref="ArgumentNullException">Thrown when instructions or method is null.</exception>
		public static IEnumerable<CodeInstruction> SkipMethod(this IEnumerable<CodeInstruction> instructions, MethodInfo method)
		{
			if (instructions == null)
			{
				throw new ArgumentNullException(nameof(instructions));
			}
			if (method == null)
			{
				throw new ArgumentNullException(nameof(method));
			}

			foreach (var instruction in instructions)
			{
				if (!instruction.Calls(method))
				{
					yield return instruction;
				}
			}
		}

		/// <summary>
		/// Skips loading the field and the next proceeding method call a specific number of times.
		/// </summary>
		/// <param name="instructions">The instructions to transpile.</param>
		/// <param name="field">The target field.</param>
		/// <param name="method">The target method.</param>
		/// <param name="count">The number of times to skip the field-method pair.</param>
		/// <returns>The transpiled instructions.</returns>
		/// <exception cref="ArgumentNullException">Thrown when instructions, field or method is null.</exception>
		/// <exception cref="ArgumentOutOfRangeException">Thrown when count is less than zero.</exception>
		public static IEnumerable<CodeInstruction> SkipFieldAndMethod(this IEnumerable<CodeInstruction> instructions, FieldInfo field, MethodInfo method, int count)
		{
			if (instructions == null)
			{
				throw new ArgumentNullException(nameof(instructions));
			}
			if (field == null)
			{
				throw new ArgumentNullException(nameof(field));
			}
			if (method == null)
			{
				throw new ArgumentNullException(nameof(method));
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException(nameof(count), $"Count={count} cannot be less than zero.");
			}

			bool fieldFound = false;

			foreach (var instruction in instructions)
			{
				if (count > 0)
				{
					if (instruction.LoadsField(field))
					{
						fieldFound = true;
						continue;
					}
					else if (fieldFound && instruction.Calls(method))
					{
						fieldFound = false;
						count--;
						continue;
					}
				}
				yield return instruction;
			}
		}
	}
}
