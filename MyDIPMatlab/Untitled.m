clear;
clc;
%imageOrigin = imread('..\test\test5.jpg');
i=imread('..\test\test5.jpg');
i=rgb2gray(i);
%�������
ig=imnoise(i,'poisson');
%��ȡ����
s=GetStrelList();
%����ȥ��
edit erodeList
e=erodeList(ig,s);
edit getRateList
%����Ȩ��
f=getRateList(ig,e);
edit getRemoveResult
%����
igo=getRemoveResult(f,e);
%��ʾ���
subplot(1,2,1),imshow(f);
subplot(1,2,1),imshow(i);
title('ԭͼ��');
subplot(1,2,2),imshow(ig),title('����ͼ��');
figure;
subplot(2,2,1),imshow(e.eroded_co12);title('����1������');
subplot(2,2,2),imshow(e.eroded_co22);title('����2������');
subplot(2,2,3),imshow(e.eroded_co32);title('����3������');
subplot(2,2,4),imshow(e.eroded_co42);title('����4������');
figure;
subplot(1,2,1),imshow(ig),title('����ͼ��');
subplot(1,2,2),imshow(igo),title('����ȥ��ͼ��');
