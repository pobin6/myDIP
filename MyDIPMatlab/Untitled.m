clear;
clc;
numSingle = ones(1,1000);
i = 2;
index = 1;
p = 18;
while i < 2^p
    if isprime(i) == 1
        numSingle(index) = i;
        index = index + 1;
    end
    i = i + 1;
end
bin = dec2bin(numSingle);
numS = ones(p,1);
numS2 = ones(p,1);
numS3 = ones(p,1);
for i = 1:p
    numS(i) = sum(numSingle<2^i & numSingle>=2^(i-1));
    numS2(i) = sum(numSingle<=2^i & numSingle>2^(i-1));
    numS3(i) = 2^(i-1);
end