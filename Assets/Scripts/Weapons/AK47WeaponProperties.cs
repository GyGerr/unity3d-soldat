using System;

public class AK47WeaponProperties : WeaponProperties {

	public AK47WeaponProperties() : base()
	{
		this.rangeMax = 100.0f;
		
		this.rof = 700;
		
		this.damageOverRange = new float[2, 3];
		damageOverRange = new float[,] { { 0, 18 }, { 15, 12 }, { 45, 8 } };
		
		this.damageMultipliers = new float[] { 3.0f, 1.0f, 0.7f };
	}
}
