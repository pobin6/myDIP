clc
clear 
A1=imread('..\project2\document1.bmp');
A1=rgb2gray(A1);
H=im2bw(A1);
J =~ H;
B = [true,true,false,false,false,false,true,true;true,false,false,true,true,false,false,true;true,false,true,true,true,false,false,true;false,false,true,true,true,true,true,true;false,false,true,true,true,true,true,true;true,false,false,true,true,true,false,true;true,false,false,false,true,false,false,true;true,true,false,false,false,false,true,true];
K=imerode(H,B);%������1ȥ��ʴԭͼ
L=imerode(J,B);%������2ȥ��ʴԭͼ�Ĳ���
K1=mat2gray(K);
L1=mat2gray(L);
R=mat2gray(H-K);%ԭͼֱ�Ӽ�ȥ������1��ʴ�Ľ��
M1=K-L;%���л����б任
M2=abs(K-L);%���л����б任�ľ���ֵ
R1=mat2gray(M1);
% R2=mat2gray(M2);%
R3=mat2gray(~M2);%���л����б任����ֵȡ��
figure(1);
subplot(231),imshow(H),title('ԭͼ�Ҷ�ͼ');
subplot(232),imshow(K1);title('����1��ʴ���');
subplot(233),imshow(L1),title('����2��ʴ���');
subplot(234),imshow(R),title('ԭͼֱ�Ӽ�ȥ��ʴ2���');
subplot(235),imshow(R1),title('���л����б任���');
subplot(236),imshow(R3),title('���л����б任ȡ����ֵ��ȡ�����');
% % ����
% A2=imdilate(A1,B);
% figure
% imshow(A2);
% ��ʴ
% A3=imerode(A1,B);
% figure
% imshow(A3);
% % ��
% A4=imopen(A1,B);
% figure
% imshow(A4);
% % ��
% A5=imclose(A1,B);
% figure
% imshow(A5);