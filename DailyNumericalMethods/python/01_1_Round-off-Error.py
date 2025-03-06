a = 0.1 #默认为64bit float
b = 0.2

print( a + b ) #0.30000000000000004
print( a + b == 0.3) #False

import numpy as np
a = np.float32(a)
b = np.float32(b)

print( a + b ) #0.3
print( a + b == np.float32(0.3)) #True