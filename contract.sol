pragma solidity ^0.4.0;

contract sensors
{
    string public show_me;
    struct sensors_value
    {
        string temp;
        string gas;                         // old struct 
        string motion;
        string time;
    }
    
    struct sensors_reads
    {
        string name;                    /// new struct holds sensors reads
        string value;
        string time;
    }
    
    struct sensor_data 
    {
        string sensor_name;
        string sensor_V;                // struct holds sensors basic info 
        string sensor_S;
        string sensor_N;
    }
    
    mapping(uint => sensor_data) public sensor_data_struct;
    mapping(uint => sensors_value) public sensors_struct;       // mapping struct to be countable
    mapping(uint => sensors_reads) public sensors_reads_sturct;
    
    uint sensors_struct_count;
    uint sensor_data_struct_count;
    uint sensors_reads_sturct_count;
    
    function set_reads(string _name,string _value,string _time) returns(int)
    {
            sensors_reads_sturct_count++;               // new method to set sensors data to structs
            sensors_reads_sturct[sensors_reads_sturct_count] = sensors_reads(_name,_value,_time);
    }
    
    function set_values(string _temp,string _gas,string _motion,string _time) returns(int)
    {
        sensors_struct_count++;
        sensors_struct[sensors_struct_count] = sensors_value(_temp,_gas,_motion,_time);  // old method to set sensors data to structs
        return 1;
    }
    
    function set_sensor_data(string _name,string _V,string _S, string _N) returns(int)
    {
        sensor_data_struct_count++;
        sensor_data_struct[sensor_data_struct_count] = sensor_data(_name,_V,_S,_N);  // method to set sensor basic info
        show_me = _name;
        return 1;
    }
    
    function update_sensor_data(string _name,string _V,string _S, string _N) returns(int)
    {
        for (uint i = 1; i <= sensor_data_struct_count;i++)
        {
            sensor_data temp = sensor_data_struct[i];
            if(stringsEqual(temp.sensor_name,_name))
            {                                                               // method to update keys for sensor
                temp.sensor_V = _V;
                temp.sensor_S = _S;
                temp.sensor_N = _N;
                return 1;
            }
        }
    }
    
    function get_sensor_key_V(string _name) returns(string)
    {
        for (uint i = 1; i <= sensor_data_struct_count;i++)
        {
            sensor_data temp = sensor_data_struct[i];               // get key
            if(stringsEqual(temp.sensor_name,_name))
            {
                show_me = temp.sensor_V;
                return temp.sensor_V;
                
            }
        }
    }
    
    function get_sensor_key_S(string _name) returns(string)
    {
        for (uint i = 1; i <= sensor_data_struct_count;i++)             // get key
        {
            sensor_data temp = sensor_data_struct[i];
            if(stringsEqual(temp.sensor_name,_name))
            {
                return temp.sensor_S;
                
            }
        }
    }
    
    function get_sensor_key_N(string _name) returns(string)
    {
        for (uint i = 1; i <= sensor_data_struct_count;i++)                         // get key
        {
            sensor_data temp = sensor_data_struct[i];
            if(stringsEqual(temp.sensor_name,_name))
            {
                return temp.sensor_N;
                
            }
        }
    }

    function get_sensors_data(uint _index) returns (string,string,string,string)
    {
        sensors_value obj = sensors_struct[_index];
        return (obj.temp,obj.gas,obj.motion,obj.time);
    }
    
     function get_sensors_reads(uint _index) returns (string,string,string)
    {
        sensors_reads obj = sensors_reads_sturct[_index];
        return (obj.name,obj.value,obj.time);
    }
    
    function get_sensors_data_count() returns(uint)
    {
        return sensors_struct_count;
    }
    
    function stringsEqual(string storage _a, string memory _b) internal returns (bool) {
		bytes storage a = bytes(_a);
		bytes memory b = bytes(_b);
		if (a.length != b.length)
			return false;
		// @todo unroll this loop
		for (uint i = 0; i < a.length; i ++)
			if (a[i] != b[i])
				return false;
		return true;
	}
    

}