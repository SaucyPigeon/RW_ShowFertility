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
		/// <exception cref="ArgumentNullException">Thrown when instructions, target or value are null.</exception>
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
		/// <exception cref="ArgumentNullException">Thrown when instructions, target or value are null.</exception>
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
	}
}
