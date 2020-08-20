from web3 import Web3, HTTPProvider, IPCProvider, WebsocketProvider
from random import randint
from datetime import datetime	
import time
import json

# ---- main -------
# Author @Lavega Team 

#-- Read abi , contracts

web3 = Web3(HTTPProvider('http://127.0.0.1:8545'))
con_abi = open('con_abi','r')
con_abi = con_abi.read()
con_add = open('con_add','r')
#con_add = con_add.read()
con_add = '0xA7045755541934Aef0ca64F11a2c610E6C6efa00'
con_byte_code = open('con_byte_code','r')
con_byte_code = con_byte_code.read()


web3.eth.defalutAccount = web3.eth.accounts[0]
con_add = web3.toChecksumAddress(con_add)
bid_abi=json.loads(con_abi)
contract = web3.eth.contract(abi=con_abi,address = con_add)

print('''

1- stream data
2- get data


''')

c = input('->')

if c == '1':

    while(True):
        time.sleep(3)  # 3 sec
        _temp = str(randint(0,100))
        _gas = str(randint(0,1))
        _motion = str(randint(0,1))
        _time = str(datetime.now())
        transact = contract.functions.set_values(_temp,_gas,_motion,_time).transact({'from': web3.eth.defalutAccount}) # transaction
        result = contract.functions.set_values(_temp,_gas,_motion,_time).call()
        result = str(result)
        print(result)
        if result == '1':
            print(_temp)
            print(_gas)
            print(_motion)
            print(_time)
            print('on block-chain')
        else:
            print('Error')

elif c == '2':
    transact = contract.functions.get_sensors_data_count().transact({'from': web3.eth.defalutAccount}) # transaction
    result = contract.functions.get_sensors_data_count().call()
    sensors_data_count = int(result)
    print (result)

    for x in range(1,(sensors_data_count+1)):
        transact = contract.functions.get_sensors_data(x).transact({'from': web3.eth.defalutAccount}) # transaction
        data = contract.functions.get_sensors_data(x).call()
        data = str(data)
        print(data)


