clear;
clc;
%�˲�����
%1����չԭͼ�񣬾�����ԭ������
%2����(-1)^(x+y) �� ���
%3������Ҷ�任�����˲����˻�
%4����(-1)^(x+y)�븵��Ҷ���任����˲����ʵ���˻�
%5���ü�ͼ��
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
% 3 ����Ҷ�任
imageDouble = im2double(imagePQ);
F = fft2(imageDouble);      % ����Ҷ�任
% 4 Ӧ���˲��� �� ����
G = Bandpass_Filter(F,355,40);

imageRes = real(ifft2(G));
imageRes(1:2:P,1:2:Q) = imageRes(1:2:P,1:2:Q) * 1;
imageRes(2:2:P,2:2:Q) = imageRes(2:2:P,2:2:Q) * 1;
imageRes(1:2:P,2:2:Q) = imageRes(1:2:P,2:2:Q) * -1;
imageRes(2:2:P,1:2:Q) = imageRes(2:2:P,1:2:Q) * -1;
%F = abs(F);                 % ����Ҷ�任���Ǹ��������㸴����ģ
%T=log(F+1);                 % ���ж����任 ��ͨģ�� ��ͨ��
figure();
imshow(imageRes,[])
