#Program to count the positive bits of a decimal number representation in binary given a binary number

#Function to determine if the value of each bit is wether 0 or 1
def assignZeroOrOne(BINARYNUMBER, REMAINDER):
    if REMAINDER == 0:
        BINARYNUMBER.append(0)
    elif REMAINDER != 0:
        BINARYNUMBER.append(1)
    return BINARYNUMBER

#Function to flip the fliped binary number to have it on it's normal stat
def flipArray(ARRAY):
    for i in range(0, int(len(ARRAY) / 2), 1):
        PIVOT = ARRAY[i]
        ARRAY[i] = ARRAY[len(ARRAY) - 1 - i]
        ARRAY[len(ARRAY) - 1 - i] = PIVOT
    return ARRAY

#It determines the ammount of one's present in a binary number
def countDecimalNumberHighBits(DECIMAL):
    if DECIMAL >= 0:
        BINARYNUMBER = decimalToBinary(DECIMAL)
        CDR = 0
        for i in range(0, len(BINARYNUMBER), 1):
            if BINARYNUMBER[i] == 1:
                CDR += 1
    else:
        CDR = None
        print("Can't convert negative numbers")
    
    return CDR

#Function to convert from decimal to binary
def decimalToBinary(DECIMALNUMBER):
    QUOTIENT = 1
    BINARYNUMBER = []
    while QUOTIENT != 0:
        QUOTIENT= int( DECIMALNUMBER / 2)
        REMAINDER = DECIMALNUMBER % 2
        DECIMALNUMBER = QUOTIENT
        assignZeroOrOne(BINARYNUMBER, REMAINDER)

    BINARYNUMBER = flipArray(BINARYNUMBER)
    return BINARYNUMBER

#Let's print first 10 decimal to binary numbers and the amount of HIGH bits necesary to express each decimal in binary
for i in range(0, 20, 1):
    binaryNumber = decimalToBinary(i)
    cdr = countDecimalNumberHighBits(i)
    print(f"Decimal {i} in binary is: {binaryNumber}")
    print(f"On its binary representation it has: {cdr} one's")
    print("")
