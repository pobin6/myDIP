clear;
clc;
%�˲�����
%1����չԭͼ�񣬾�����ԭ������
%2����(-1)^(x+y) �� ���
%3������Ҷ�任�����˲����˻�
%4����(-1)^(x+y)�븵��Ҷ���任����˲����ʵ���˻�
%5���ü�ͼ��
imageOrigin = imread('..\test\test5.jpg');
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
% 3 ����Ҷ�任
imageDouble = im2double(imagePQ);
F = fft2(imageDouble);      % ����Ҷ�任
figure;
T=log(F+1);                 % ���ж����任 ��ͨģ�� ��ͨ��
T = real(T);                 % ����Ҷ�任���Ǹ��������㸴����ģ
imshow(T,[]);
% 4 Ӧ���˲��� �� ����
[G, uk, vk] = Notch_Filter(F,11,1);
T = G;
%T(uk-30:uk+30,vk-30:vk+30) = 0;
figure;
T=log(T+1);                 % ���ж����任 ��ͨģ�� ��ͨ��
T = real(T);                 % ����Ҷ�任���Ǹ��������㸴����ģ
imshow(T,[]);
imageRes = real(ifft2(G));
imageRes(1:2:P,1:2:Q) = imageRes(1:2:P,1:2:Q) * 1;
imageRes(2:2:P,2:2:Q) = imageRes(2:2:P,2:2:Q) * 1;
imageRes(1:2:P,2:2:Q) = imageRes(1:2:P,2:2:Q) * -1;
imageRes(2:2:P,1:2:Q) = imageRes(2:2:P,1:2:Q) * -1;
%F = abs(F);                 % ����Ҷ�任���Ǹ��������㸴����ģ
%T=log(F+1);                 % ���ж����任 ��ͨģ�� ��ͨ��
figure();
image = imageRes(1:P/2,1:Q/2);
imgMin = min(min(image));
imgMax = max(max(image));
image = uint8((image - imgMin) / (imgMax-imgMin) * 255);
imshow(image);
%imwrite(image,'..\test\test4Res.bmp');