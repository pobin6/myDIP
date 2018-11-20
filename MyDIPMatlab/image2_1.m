clear;
clc;
%滤波流程
%1、扩展原图像，经典是原来两倍
%2、用(-1)^(x+y) 与 相乘
%3、傅里叶变换后与滤波器乘积
%4、用(-1)^(x+y)与傅里叶反变换后的滤波后的实部乘积
%5、裁剪图像
imageOrigin = imread('..\test\test3.jpg');
imageGray = rgb2gray(imageOrigin);
[w,h] = size(imageGray);
P = 2*w;
Q = 2*h;
% 1
imagePQ = zeros(P,Q);
imagePQ(1:w,1:h) = imageGray;
% 2
imagePQ(1:2:P,1:2:Q) = imagePQ(1:2:P,1:2:Q) * 1;
imagePQ(2:2:P,2:2:Q) = imagePQ(2:2:P,2:2:Q) * 1;
imagePQ(1:2:P,2:2:Q) = imagePQ(1:2:P,2:2:Q) * -1;
imagePQ(2:2:P,1:2:Q) = imagePQ(2:2:P,1:2:Q) * -1;
% 3 傅里叶变换
imageDouble = im2double(imagePQ);
F = fft2(imageDouble);      % 傅里叶变换
% 4 应用滤波器 并 计算
G = Bandpass_Filter(F,355,40);

imageRes = real(ifft2(G));
imageRes(1:2:P,1:2:Q) = imageRes(1:2:P,1:2:Q) * 1;
imageRes(2:2:P,2:2:Q) = imageRes(2:2:P,2:2:Q) * 1;
imageRes(1:2:P,2:2:Q) = imageRes(1:2:P,2:2:Q) * -1;
imageRes(2:2:P,1:2:Q) = imageRes(2:2:P,1:2:Q) * -1;
%F = abs(F);                 % 傅里叶变换后是复数，计算复数的模
%T=log(F+1);                 % 进行对数变换 低通模糊 高通锐化
figure();
imshow(imageRes,[])
