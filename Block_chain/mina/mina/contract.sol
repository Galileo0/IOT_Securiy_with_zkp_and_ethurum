pragma solidity ^0.4.0;

contract sensors
{
    struct sensors_value
    {
        string temp;
        string gas;
        string motion;
        string time;
    }
    mapping(uint => sensors_value) public sensors_struct;
    uint sensors_struct_count;

    function set_values(string _temp,string _gas,string _motion,string _time) returns(int)
    {
        sensors_struct_count++;
        sensors_struct[sensors_struct_count] = sensors_value(_temp,_gas,_motion,_time);
    }

    function get_sensors_data(uint _index) returns (string,string,string,string)
    {
        sensors_value obj = sensors_struct[_index];
        return (obj.temp,obj.gas,obj.motion,obj.time);
    }
    
    function get_sensors_data_count() returns(uint)
    {
        return sensors_struct_count;
    }

}