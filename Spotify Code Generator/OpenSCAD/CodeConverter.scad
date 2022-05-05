codeheight = 4;
baseheight = 4;
keychain = false;
$fn=50;
filepath = "code.svg";
scale(0.35)
{
    translate([0,0,codeheight/2])
    linear_extrude(height=codeheight,center=true)
    import(file = filepath, center = true);

    translate([0,0,-baseheight/2])
    cube([168,50,baseheight], center = true);

    translate([-168/2,0,-baseheight/2])
    difference() 
    {
      cylinder(r=25, h=baseheight, center = true);
      translate([25,0,0]) cube(50, center=true);
    }

    translate([168/2,0,-baseheight/2])
    difference() 
    {
      cylinder(r=25, h=baseheight, center = true);
      translate([-25,0,0]) cube(50, center=true);
    }
    if (keychain)
    {
        translate([2,0,0]);
        translate([-115,0,-baseheight/2])
        difference()
        {
           cylinder(r=16, h=baseheight, center=true);
           cylinder(r=8, h=baseheight+1, center=true);
        }
        translate([-105,12,-baseheight/2]) 
        cube([20,8,baseheight], center=true);
        translate([-105,-12,-baseheight/2]) 
        cube([20,8,baseheight], center=true);
    }
}



